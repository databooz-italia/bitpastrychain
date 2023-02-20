import { ReactNode, useContext, useEffect, useState } from "react"
import SearchBar from "../components/SearchBar"
import {
	DataGrid,
	GridColDef,
	GridColumnHeaderParams,
	GridFilterItem,
	GridRenderCellParams
} from "@mui/x-data-grid"
import useAxios from "package/hooks/useAxios"
import { Ricetta, Ordine } from "../package/types"
import { ErrorHandler } from "package/Helpers"
import useAuth from "package/hooks/useAuth"
import { useNavigate } from "react-router-dom"
import AppContext from "context/AppContext"
import AlertRowModal from "components/AlertRowModal"

type Type = Ricetta & { Quantità: number }
export default function SemiLavorato() {
	const _axios = useAxios()
	const { auth } = useAuth()
	const { setWhere, where } = useContext(AppContext)
	const navigate = useNavigate()
	const [rows, setRows] = useState<Type[]>([])

	// Recupero Dati
	useEffect(() => {
		const fetchData = async () => {
			// Recupero tutti i Semi Lavorati
			const result = await _axios.get("api/ricette", {
				params: { onlySemiLavorati: true }
			})
			setRows(result.data)
		}

		try {
			fetchData()
		} catch (e) {
			ErrorHandler(e)
		}
	}, [_axios])

	/* --------------------------------------------------------------------------------------------- */
	// Handler Submit Preparazione Semi Lavorato
	const handlerSubmit = async (target: Type) => {
		// Ordine creato con successo, reindirizzo l'Operatore alla Preparazione
		// Se l'Utente ha già in carico una lavorazione uscirà il Modal che lo avvertirà
		try {
			const _do = async () => {
				// Creo l'Ordine
				const { data: idOrdine }: { data: number } =
					await _axios.post(
						"api/ordini",
						JSON.stringify({
							RicettaID: target.id,
							Quantità: target.Quantità,
							RifCliente: auth.FullName,
							ContattoCliente: `Operatore ${auth.Username}`
						} as Ordine)
					)

				// Prendo in Carico l'Ordine appena creato
				await _axios.put(`api/presa-carico/${idOrdine}`)
				setWhere("Preparazione")
				navigate("/")
			}

			// L'Operatore ha altri Ordini in sospeso, gli chiedo se li vuole rilasciare
			if (where === "Preparazione") {
				setAlert(
					<AlertRowModal
						callback={async (result: boolean) => {
							// IF result == true
							//    Utente acconsente all'Eliminazione
							// ELSE
							//    Evento annullato
							if (result) {
								await _axios.delete("api/presa-carico")
								await _do()
							}

							setAlert(undefined)
						}}
					/>
				)
			} else await _do()
		} catch (e) {
			ErrorHandler(e)
		}
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [filter, setFilter] = useState<GridFilterItem[]>([])
	const [Alert, setAlert] = useState<ReactNode>(undefined)

	return (
		<>
			<main className='w-full h-full min-h-full bg-[#e4e5e6] p-16'>
				<div className='w-full h-full bg-white border border-gray-300 rounded-lg shadow-md flex flex-col overflow-hidden'>
					<header className='text-center p-8 bg-my-grigio border-b border-gray-300'>
						<h3 className='font-bold text-4xl'>
							Prepara Semilavorato
						</h3>
					</header>
					<div className='h-full py-8 px-14 flex flex-col'>
						<h3 className='text-center text-xl font-semibold mb-2'>
							Filtra per Nome
						</h3>
						<SearchBar
							placeholder='Filtra per Nome'
							onChange={e =>
								setFilter([
									{
										columnField: "Nome",
										operatorValue: "contains",
										value: e.currentTarget.value
									}
								])
							}
						/>
						<div className='w-full h-full flex mt-16'>
							<DataGrid
								hideFooter
								disableDensitySelector
								disableColumnMenu
								disableSelectionOnClick
								filterModel={{
									items: filter
								}}
								rows={rows}
								columnVisibilityModel={{ id: false }}
								columns={
									[
										{
											field: "id",
											headerName: "ID",
											flex: 0.5,
											editable: false,
											sortable: false,
											filterable: false
										},
										{
											field: "Nome",
											headerName: "Semilavorato",
											flex: 1,
											editable: false
										},
										{
											// TODO : Si dovrà fare?
											field: "InMagazzino",
											headerName: "In Magazzino",
											type: "number",
											headerAlign: "left",
											align: "left",
											flex: 0.5,
											editable: false
										},
										{
											field: "UnitàMisura",
											headerName: "Unità di Misura",
											flex: 0.4,
											editable: false
										},
										{
											field: "Quantità",
											headerName: "Quantità",
											flex: 1,
											editable: false,
											renderCell: (
												props: GridRenderCellParams
											) => (
												<input
													className='rounded-md border-2 border-gray-300 relative block w-full appearance-none px-3 py-2 text-gray-900 placeholder-gray-500 focus:z-10 focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm'
													id='Quantità'
													type='number'
													step='any'
													min='0'
													defaultValue={
														props.row.Quantità
													}
													onChange={e =>
														setRows(prev => {
															const _semiLavorato =
																prev.find(
																	x =>
																		x.id ===
																		props.id
																)
															if (
																_semiLavorato !==
																undefined
															)
																_semiLavorato.Quantità =
																	+e.currentTarget
																		.value

															return prev
														})
													}
												/>
											)
										},
										{
											field: "actions",
											align: "center",
											flex: 0.5,
											sortable: false,
											filterable: false,
											renderHeader: (
												_: GridColumnHeaderParams
											) => null,
											renderCell: (
												props: GridRenderCellParams
											) => (
												<i
													className='bx bx-send my-bx text-my-green text-3xl'
													onClick={() =>
														handlerSubmit(props.row)
													}
												/>
											)
										}
									] as GridColDef[]
								}
							/>
						</div>
					</div>
					<footer className='p-8 bg-my-grigio flex items-center justify-between border-t border-gray-300' />
				</div>
			</main>
			{Alert}
		</>
	)
}
