import { useState } from "react";
import { Navigate, Outlet } from "react-router-dom";

import SideBar from "../../components/SideBar";
import useAuth from "../hooks/useAuth";

function App() {
	// Per controllare se l'utente è loggato
	const { auth } = useAuth();
	// Utilizzato per aprire e chiudere la Side Bar
	const [isSideBarOpened, setSideBar] = useState<boolean>(false);

	/* --------------------------------------------------------------------------------------------- */
	// IF (utente loggato)
	//    Renderer App
	// ELSE
	//    Redirect
	if (auth)
		return (
			<>
				<SideBar
					hook={isSideBarOpened}
					setHook={setSideBar}
				/>
				<main
					className='App h-full min-h-full'
					style={{
						marginLeft: isSideBarOpened ? "250px" : "78px",
						transition: "margin-left .4s",
					}}
				>
					<Outlet />
				</main>
			</>
		);
	else
		return (
			<Navigate
				to='/login'
				replace
			/>
		);
}

export default App;
