import { Navigate, Outlet } from "react-router-dom";
import useAuth from "../hooks/useAuth";

// Tutti i casi:
//  - notRequireAuth | utente Loggato     => Redirect
//  - notRequireAuth | utente non Loggato => Va bene
//  - requireAuth    | Utente Loggato     => Va bene
//  - requireAuth    | Utente non Loggato => Redirect
export function NotRequireAuth() {
	const { auth } = useAuth();

	// IF (utente loggato)
	//    Redirect
	// ELSE
	//    Render page
	if (auth)
		return (
			<Navigate
				to='/'
				replace
			/>
		);
	return <Outlet />;
}
