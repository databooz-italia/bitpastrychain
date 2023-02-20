import {
	Dispatch,
	SetStateAction,
	createContext,
	useState,
} from "react";
import config from "package/config.json";
import { User } from "../types";

import "../main.css";

/* --------------------------------------------------------------------------------------------- */
// Context
const AuthContext = createContext({});
export interface ContextProps {
	auth: User;
	setAuth: Dispatch<SetStateAction<User | undefined>>;
}

/* --------------------------------------------------------------------------------------------- */
// Provider
export const AuthProvider = ({
	children,
}: {
	children: React.ReactNode;
}) => {
	// Definisco l'istanza del contenuto che il mio Contesto dovrà contenere
	const [state, setState] = useState<User | undefined>(
		getLocalStorageUser(),
	);

	// Ritorno il children Wrappato dal contesto
	return (
		<AuthContext.Provider
			value={{
				auth: state,
				setAuth: setState,
			}}
		>
			{children}
		</AuthContext.Provider>
	);
};

export default AuthContext;

/* --------------------------------------------------------------------------------------------- */
// Recupero l'user dallo storage locale, in questo modo posso far funzionare il "Remember me"
function getLocalStorageUser(): User | undefined {
	try {
		const temp: string | null = localStorage.getItem(
			config.KEYS.USER,
		);
		if (temp) {
			const user: User = JSON.parse(temp);

			// Se l'oggetto è integro lo restituisco, altrimenti restituisco undefined
			if (user.Username && user.id && user.Token) return user;
			else localStorage.removeItem(config.KEYS.USER);
		}
	} catch (error) {
		console.error(
			"Errore nel recupero dell'User dal Local Storage: " + error,
		);
	}
}
