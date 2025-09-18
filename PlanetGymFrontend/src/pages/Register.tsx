import { useState } from 'react'
import api from '../api'
import { useAuth } from '../store/auth'


export default function Register() {
const [fullName, setFullName] = useState('')
const [email, setEmail] = useState('')
const [password, setPassword] = useState('')
const [loading, setLoading] = useState(false)
const [err, setErr] = useState('')
const setAuth = useAuth((s) => s.setAuth)


async function submit(e: React.FormEvent) {
e.preventDefault(); setErr(''); setLoading(true)
try {
const { data } = await api.post('/api/auth/register', { fullName, email, password })
setAuth(data.token, { fullName: data.fullName, email: data.email, role: data.role })
location.href = '/classes'
} catch (e: any) {
setErr(e?.response?.data || 'Registration failed')
} finally {
setLoading(false)
}
}


return (
<div className="mx-auto max-w-md p-6">
<h1 className="text-2xl font-semibold mb-4">Create your account</h1>
<form onSubmit={submit} className="space-y-3">
<input className="w-full border rounded-xl px-3 py-2" placeholder="Full name" value={fullName} onChange={e => setFullName(e.target.value)} />
<input className="w-full border rounded-xl px-3 py-2" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
<input className="w-full border rounded-xl px-3 py-2" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
{err && <p className="text-red-600 text-sm">{err}</p>}
<button disabled={loading} className="w-full rounded-xl px-3 py-2 bg-violet-700 text-white hover:bg-violet-600 disabled:opacity-50">{loading ? 'Creating…' : 'Sign up'}</button>
</form>
</div>
)
}