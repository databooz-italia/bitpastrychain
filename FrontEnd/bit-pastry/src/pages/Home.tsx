import { useContext } from "react"
import Preparazione from "./Preparazione"
import Ordini from "./Ordini"
import AppContext from "../context/AppContext"

export default function Home() {
	const { where, setWhere } = useContext(AppContext)

	// IF exist ID
	//    Preparazione dell'Ordine
	// ELSE
	//    Tutti gli Ordini Aperti
	return where === "Ordini" ? (
		<Ordini setWhere={setWhere} />
	) : (
		<Preparazione setWhere={setWhere} />
	)
}
