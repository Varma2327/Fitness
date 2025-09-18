import { Routes, Route, Navigate } from "react-router-dom"
import Navbar from "./components/Navbar"
import Home from "./pages/Home"
import Locations from "./pages/Locations"
import Classes from "./pages/Classes"
import Logs from "./pages/Logs"
import Login from "./pages/Login"
import Register from "./pages/Register"
import { useAuth } from "./store/auth"

function Private({ children }: { children: JSX.Element }) {
  const token = useAuth(s => s.token)
  return token ? children : <Navigate to="/login" replace />
}

export default function App() {
  return (
    <div>
      <Navbar />
      <div className="pt-4">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/locations" element={<Locations />} />
          <Route path="/classes" element={<Private><Classes /></Private>} />
          <Route path="/logs" element={<Private><Logs /></Private>} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </div>
  )
}
