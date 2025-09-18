import { useEffect, useState } from "react"
import api from "../api"

type LogItem = {
  id: number
  type: string
  durationMinutes: number
  calories: number
  dateUtc: string
  notes: string
}

export default function Logs() {
  const [items, setItems] = useState<LogItem[]>([])
  const [type, setType] = useState("Cardio")
  const [duration, setDuration] = useState<number | "">("")
  const [calories, setCalories] = useState<number | "">("")
  const [notes, setNotes] = useState("")

  async function load() {
    const r = await api.get("/api/workoutlogs")
    setItems(r.data)
  }
  useEffect(() => { load() }, [])

  async function add() {
    await api.post("/api/workoutlogs", {
      type,
      durationMinutes: Number(duration || 0),
      calories: Number(calories || 0),
      notes
    })
    setDuration(""); setCalories(""); setNotes("")
    load()
  }

  return (
    <div className="mx-auto max-w-6xl p-6">
      <h1 className="text-2xl font-bold text-brand-red mb-4">My Logs</h1>

      <div className="card p-4 mb-6">
        <div className="grid gap-3 sm:grid-cols-4">
          <select value={type} onChange={e => setType(e.target.value)} className="input">
            <option>Cardio</option><option>Strength</option><option>Yoga</option><option>Other</option>
          </select>
          <input className="input" type="number" placeholder="Duration (min)" value={duration}
                 onChange={e => setDuration(e.target.value === "" ? "" : Number(e.target.value))} />
          <input className="input" type="number" placeholder="Calories" value={calories}
                 onChange={e => setCalories(e.target.value === "" ? "" : Number(e.target.value))} />
          <input className="input sm:col-span-4" placeholder="Notes" value={notes}
                 onChange={e => setNotes(e.target.value)} />
        </div>
        <div className="mt-4"><button onClick={add} className="btn">Add Log</button></div>
      </div>

      <div className="grid gap-3">
        {items.map(it => (
          <div key={it.id} className="card p-4">
            <div className="flex justify-between">
              <div className="font-semibold text-brand-red">{it.type}</div>
              <div className="text-xs text-white/70">{new Date(it.dateUtc).toLocaleString()}</div>
            </div>
            <div className="text-sm mt-1">{it.durationMinutes} min • {it.calories} cal</div>
            {it.notes && <div className="text-sm text-white/80 mt-1">{it.notes}</div>}
          </div>
        ))}
        {items.length === 0 && <div className="text-white/70">No logs yet. Add your first workout!</div>}
      </div>
    </div>
  )
}
