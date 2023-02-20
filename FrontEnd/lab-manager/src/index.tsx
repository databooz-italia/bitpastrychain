import React from "react"
import ReactDOM from "react-dom/client"
import { BrowserRouter, Route, Routes } from "react-router-dom"

import { AuthProvider } from "./package/context/AuthProvider"
import App from "package/components/App"
import { NotRequireAuth } from "./package/components/NotRequireAuth"

import { Login } from "./package/pages/Login"

import Ricette from "./pages/Ricette"
import ModificaRicetta from "./pages/ModificaRicetta"
import Operatori from "./pages/Operatori"
import MateriePrime from "./pages/MateriePrime"
import Ordini from "./pages/Ordini"

const root = ReactDOM.createRoot(
	document.getElementById("root") as HTMLElement
)
root.render(
	<React.StrictMode>
		<BrowserRouter>
			{/* Contesto che Wrappa tutta la mia Web-App*/}
			<AuthProvider>
				<Routes>
					{/* Routering quando l'utente non è Loggato  */}
					<Route element={<NotRequireAuth />}>
						<Route
							path='/login'
							element={<Login url='auth/login' />}
						/>
					</Route>

					{/* Routering quando l'utente è Loggato  */}
					<Route element={<App />}>
						<Route
							path='/ricette'
							element={<Ricette />}
						/>
						<Route
							path='/ricette/:RicettaID'
							element={<ModificaRicetta />}
						/>
						<Route
							path='/operatori'
							element={<Operatori />}
						/>
						<Route
							path='/materie-prime'
							element={<MateriePrime />}
						/>
						<Route
							path='*'
							element={<Ordini />}
						/>
					</Route>
				</Routes>
			</AuthProvider>
		</BrowserRouter>
	</React.StrictMode>
)
