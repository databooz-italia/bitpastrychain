import {
	createContext,
	useState,
	Dispatch,
	SetStateAction
} from "react"

import App from "package/components/App"

/* --------------------------------------------------------------------------------------------- */
// Context
const AppContext = createContext<ContextProps>({
	setWhere: () => {},
	where: "Ordini"
})

export type Where = "Preparazione" | "Ordini"
export type ContextProps = {
	where: Where
	setWhere: Dispatch<SetStateAction<Where>>
}

/* --------------------------------------------------------------------------------------------- */
// Provider
export const AppProvider = () => {
	// Definisco l'istanza del contenuto che il mio Contesto dovrà contenere
	const [state, setState] = useState<Where>("Ordini")

	// Ritorno il children Wrappato dal contesto
	return (
		<AppContext.Provider
			value={{
				where: state,
				setWhere: setState
			}}
		>
			<App />
		</AppContext.Provider>
	)
}

export default AppContext
