import { useContext } from "react";
import AuthContext, { ContextProps } from "../context/AuthProvider";
import { User } from "../types";
import config from "package/config.json";

const useAuth = () => {
	const { auth } = useContext(AuthContext) as ContextProps;

	// Log In: Salvo l'utente all'interno dello Storage del Browser
	const handlerLogIn = (user: User) => {
		localStorage.setItem(config.KEYS.USER, JSON.stringify(user));
		window.location.reload();
	};

	// Log Out: Rimuovo l'utente dall'Interno dello Storage del Browser
	const handlerLogOut = () => {
		localStorage.removeItem(config.KEYS.USER);
		window.location.reload();
	};

	return { auth, handlerLogIn, handlerLogOut };
};

export default useAuth;
