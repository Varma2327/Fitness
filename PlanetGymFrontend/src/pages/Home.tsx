export default function Home() {
  return (
    <div
      className="relative min-h-[82vh] w-full overflow-hidden rounded-2xl"
      style={{
        backgroundImage:
          'url("https://images.unsplash.com/photo-1517836357463-d25dfeac3438?q=80&w=1600&auto=format&fit=crop")',
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      <div className="absolute inset-0 bg-black/70" />
      <div className="relative z-10 max-w-6xl mx-auto px-6 py-16">
        <h1 className="text-5xl font-extrabold text-brand-red tracking-tight mb-3">
          TRAIN HARD
        </h1>
        <p className="text-white/90 max-w-xl">
          Push your limits. Book classes. Track progress. Become unstoppable.
        </p>

        <div className="mt-8 flex gap-4">
          <a href="/classes" className="btn">Browse Classes</a>
          <a href="/logs" className="rounded-xl px-5 py-2 border border-brand-red text-brand-red font-semibold hover:bg-brand-red/10">
            My Logs
          </a>
        </div>
      </div>
    </div>
  )
}
