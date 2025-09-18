import { Link } from "react-router-dom"
import { useAuth } from "../store/auth"

export default function Navbar() {
  const { token, logout } = useAuth()

  return (
    <nav className="bg-brand-light border-b border-brand-red">
      <div className="max-w-6xl mx-auto px-6 py-3 flex items-center justify-between">
        <Link to="/" className="font-extrabold text-brand-red text-xl">
          PlanetGym
        </Link>
        <div className="flex items-center gap-4 text-sm">
          <Link to="/" className="hover:text-brand-red">Home</Link>
          <Link to="/locations" className="hover:text-brand-red">Locations</Link>
          <Link to="/classes" className="hover:text-brand-red">Classes</Link>
          <Link to="/logs" className="hover:text-brand-red">My Logs</Link>

          {token ? (
            <button onClick={logout} className="btn">Logout</button>
          ) : (
            <div className="flex gap-2">
              <Link to="/login" className="btn">Login</Link>
              <Link
                to="/register"
                className="rounded-xl px-4 py-2 border border-brand-red text-brand-red font-semibold hover:bg-brand-red/10"
              >
                Sign Up
              </Link>
            </div>
          )}
        </div>
      </div>
    </nav>
  )
}
