import { useEffect, useState } from "react"
import api from "../api"

type Loc = { id: number; name: string; address: string; zipCode: string; timezone: string }

export default function Locations() {
  const [all, setAll] = useState<Loc[]>([])
  const [items, setItems] = useState<Loc[]>([])
  const [zip, setZip] = useState("")

  useEffect(() => {
    (async () => {
      const r = await api.get("/api/locations")
      setAll(r.data); setItems(r.data)
    })()
  }, [])

  function search() {
    const q = zip.trim()
    if (!q) return setItems(all)
    setItems(all.filter(l => l.zipCode?.startsWith(q)))
  }

  return (
    <div className="mx-auto max-w-6xl p-6">
      <h1 className="text-2xl font-bold text-brand-red mb-4">Locations</h1>

      <div className="mb-5 flex flex-wrap items-center gap-3">
        <input className="input w-40" placeholder="ZIP code" value={zip} onChange={e => setZip(e.target.value)} />
        <button onClick={search} className="btn">Search</button>
        <button onClick={() => { setZip(""); setItems(all) }} className="rounded-xl px-4 py-2 bg-neutral-900 text-white border border-brand-red hover:bg-brand-red/10">
          All
        </button>
      </div>

      <div className="grid gap-4 sm:grid-cols-2">
        {items.map(l => (
          <div key={l.id} className="card p-4">
            <div className="font-semibold text-brand-red">{l.name}</div>
            <div className="text-sm text-white mt-1">{l.address}</div>
            <div className="text-xs text-white/70 mt-1">{l.zipCode} • {l.timezone.replace("_", " ")}</div>
          </div>
        ))}
      </div>
    </div>
  )
}
