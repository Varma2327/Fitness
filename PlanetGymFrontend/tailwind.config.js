/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        brand: {
          red: "#b71c1c",   // OG deep red
          dark: "#0b0b0b",  // app background
          light: "#141414", // card background
        },
      },
    },
  },
  plugins: [],
}
