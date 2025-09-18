import { useEffect, useState } from "react"
import api from "../api"

type ClassItem = {
  id: number
  title: string
  description: string
  capacity: number
  startUtc: string
  endUtc: string
  locationId: number
  locationName: string
  booked: boolean
  spotsLeft: number
}

export default function Classes() {
  const [items, setItems] = useState<ClassItem[]>([])
  const [onlyMine, setOnlyMine] = useState(false)
  const [loading, setLoading] = useState(true)
  const [msg, setMsg] = useState<string>("")

  async function load() {
    setLoading(true); setMsg("")
    try {
      const url = onlyMine ? "/api/classes?mine=true" : "/api/classes"
      const r = await api.get(url)
      setItems(r.data)
    } catch (e: any) {
      console.error("Load classes failed:", e?.response?.status, e?.response?.data)
      setItems([])
      setMsg(typeof e?.response?.data === "string" ? e.response.data : "Failed to load classes")
    } finally { setLoading(false) }
  }
  useEffect(() => { load() }, [onlyMine])

  async function book(id: number) {
    try { await api.post("/api/classes/book", { classId: id }); setMsg("Booked."); load() }
    catch (e: any) { setMsg(e?.response?.data || "Booking failed") }
  }
  async function cancel(id: number) {
    try { await api.delete(`/api/classes/book/${id}`); setMsg("Cancelled."); load() }
    catch (e: any) { setMsg(e?.response?.data || "Cancel failed") }
  }

  return (
    <div className="mx-auto max-w-6xl p-6">
      <div className="mb-4 flex items-center gap-3">
        <label className="flex items-center gap-2 text-sm text-white/80">
          <input type="checkbox" checked={onlyMine} onChange={e => setOnlyMine(e.target.checked)} />
          Show my bookings only
        </label>
        <button onClick={load} className="btn">Refresh</button>
      </div>

      {msg && <div className="mb-3 rounded-xl bg-brand-red/15 text-brand-red px-3 py-2 text-sm">{msg}</div>}
      {loading && <div className="text-sm text-white/80">Loading…</div>}
      {!loading && items.length === 0 && <div className="text-sm text-white/80">No classes found.</div>}

      <div className="grid gap-4 sm:grid-cols-2">
        {items.map(c => (
          <div key={c.id} className="card p-4">
            <div className="flex justify-between items-start">
              <div>
                <div className="font-bold text-brand-red">{c.title}</div>
                <div className="text-xs text-white/70">{c.locationName}</div>
              </div>
              <div className="text-xs text-white/70">{c.spotsLeft} / {c.capacity} left</div>
            </div>

            <div className="text-sm mt-2">{c.description}</div>
            <div className="text-xs text-white/70 mt-2">
              {new Date(c.startUtc).toLocaleString()} → {new Date(c.endUtc).toLocaleString()}
            </div>

            <div className="mt-3 flex items-center gap-2">
              {c.booked ? (
                <>
                  <span className="rounded-full bg-brand-red/20 text-brand-red text-xs px-2 py-1">Booked</span>
                  <button onClick={() => cancel(c.id)} className="btn">Cancel</button>
                </>
              ) : (
                <button onClick={() => book(c.id)} disabled={c.spotsLeft <= 0} className="btn disabled:opacity-40">
                  {c.spotsLeft <= 0 ? "Full" : "Book"}
                </button>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
