import React from "react"
import ReactDOM from "react-dom/client"
import * as serviceWorkerRegistration from "./serviceWorkerRegistration"
import { BrowserRouter, Route, Routes } from "react-router-dom"

import { AuthProvider } from "package/context/AuthProvider"
import { AppProvider } from "./context/AppContext"
import { NotRequireAuth } from "./package/components/NotRequireAuth"

import { Login } from "./package/pages/Login"

// import SemiLavorato from "pages/SemiLavorato"
import Home from "pages/Home"

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
							element={<Login url='auth/login-operatore' />}
						/>
					</Route>

					{/* Routering quando l'utente è Loggato  */}
					<Route element={<AppProvider />}>
						{/* <Route
							path='semi-lavorato'
							element={<SemiLavorato />}
						/> */}
						<Route
							path='*'
							element={<Home />}
						/>
					</Route>
				</Routes>
			</AuthProvider>
		</BrowserRouter>
	</React.StrictMode>
)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.register()
