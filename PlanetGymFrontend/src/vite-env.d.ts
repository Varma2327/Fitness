interface ImportMetaEnv {
  readonly VITE_API_BASE_URL: string
  // add more envs here if you create them
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
