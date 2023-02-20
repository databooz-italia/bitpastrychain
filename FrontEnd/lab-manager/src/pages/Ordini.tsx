import MyTable from "package/components/MyTable"
import { Ordine } from "../package/types"
import { GridColDef } from "@mui/x-data-grid"
import { ShowDate } from "package/Helpers"
import { useEffect, useState } from "react"
import { Ricetta } from "package/types"
import useAxios from "package/hooks/useAxios"

export default function Ordini() {
	// Gestione delle Ricette
	const _axios = useAxios()
	const [optionsRicetta, setOptionsRicetta] = useState<
		Map<number, string>
	>(new Map())

	// Recupero tutte le Ricette
	useEffect(() => {
		const fetchData = async () => {
			const { data } = await _axios.get("api/ricette")
			const map = new Map()
			data.map((x: Ricetta) => map.set(x.id, x.Nome))
			setOptionsRicetta(map)
		}

		try {
			fetchData()
		} catch {}
	}, [_axios])

	/* --------------------------------------------------------------------------------------------- */
	return (
		<MyTable<Ordine>
			title='Ordini'
			columns={
				[
					{
						field: "RicettaID",
						headerName: "Ricetta",
						flex: 1,
						editable: true,
						type: "singleSelect",
						valueOptions: Array.from(optionsRicetta.keys()),
						valueGetter: params => params.value ?? "",
						valueFormatter: params =>
							optionsRicetta.get(params.value)
					},
					{
						field: "Quantità",
						headerName: "Quantità",
						flex: 1,
						editable: true,
						type: "number",
						headerAlign: "left",
						align: "left"
					},
					{
						field: "TsOrdine",
						headerName: "Data",
						flex: 1,
						editable: true,
						type: "date",
						valueGetter: params => params.value ?? new Date(),
						valueFormatter: params => ShowDate(params.value)
					},
					{
						field: "RifCliente",
						headerName: "Rif. Cliente",
						flex: 1,
						editable: true
					},
					{
						field: "ContattoCliente",
						headerName: "Contatto Cliente",
						flex: 1,
						editable: true
					}
				] as GridColDef[]
			}
			url='api/ordini'
			firstColumn='ContattoCliente'
		/>
	)
}
