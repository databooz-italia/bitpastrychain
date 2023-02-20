/** @type {import('tailwindcss').Config} */
module.exports = {
	content: ["./src/**/*.{js,jsx,ts,tsx,css}"],
	theme: {
		extend: {
			colors: {
				c: "#e0e0e0",
				"my-green": "#4ebd75",
				"my-yellow": "#ffc107",
				"my-grigio": "#f1f3f5",
				"my-cyan": "#2eacda"
			}
		}
	},
	plugins: []
}
