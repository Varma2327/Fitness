import { create } from 'zustand'


type AuthState = {
token: string | null
user: { fullName: string; email: string; role: string } | null
setAuth: (token: string, user: AuthState['user']) => void
logout: () => void
}


export const useAuth = create<AuthState>((set) => ({
token: localStorage.getItem('pg_token'),
user: localStorage.getItem('pg_user') ? JSON.parse(localStorage.getItem('pg_user')!) : null,
setAuth: (token, user) => {
localStorage.setItem('pg_token', token)
localStorage.setItem('pg_user', JSON.stringify(user))
set({ token, user })
},
logout: () => {
localStorage.removeItem('pg_token')
localStorage.removeItem('pg_user')
set({ token: null, user: null })
}
}))