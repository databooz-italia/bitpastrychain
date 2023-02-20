import {
	DataGrid,
	GridColDef,
	GridColumnHeaderParams,
	GridPagination,
	GridRenderCellParams,
	GridRowId,
	GridToolbarFilterButton
} from "@mui/x-data-grid"
import { ReactNode, useEffect, useState } from "react"

import useAxios from "package/hooks/useAxios"
import { useNavigate } from "react-router-dom"
import { Ricetta } from "package/types"
import MyTableWrapper from "../package/components/MyTableWrapper"
import { ShowDate } from "../package/Helpers"
import DeleteRowModal from "package/components/modals/DeleteRowModal"

const url = "api/ricette"

export default function Ricette() {
	// Axios con Auth
	const _axios = useAxios()
	const [rows, setRows] = useState<Ricetta[]>([])
	const navigate = useNavigate()

	// Popolo i dati
	useEffect(() => {
		const fetchData = async () => {
			const result = await _axios.get(url)
			setRows(result.data as Ricetta[])
		}

		fetchData()
	}, [_axios])

	/* --------------------------------------------------------------------------------------------- */
	// Handler Delete Click
	const processDeleteRow = (id: GridRowId) => {
		setAlert(
			<DeleteRowModal
				deleteMessage="Confermi di voler cancellare definitivamente la riga selezionata, e tutti gli Ordini associati a questa Ricetta? L'azione è irreversibile."
				callback={async (result: boolean) => {
					// IF result == true
					//    Utente acconsente all'Eliminazione
					// ELSE
					//    Evento annullato
					if (result) {
						const deleteData = async () => {
							// Eliminazione definitiva della Row
							await _axios.delete(`${url}/${id}`)

							setRows(prev =>
								prev.filter(row => row.id !== id)
							)
						}

						deleteData()
					}

					setAlert(undefined)
				}}
			/>
		)
	}

	/* --------------------------------------------------------------------------------------------- */
	const [Alert, setAlert] = useState<ReactNode>(undefined)

	return (
		<>
			<MyTableWrapper title='Ricette'>
				<DataGrid
					disableDensitySelector
					disableColumnMenu
					disableSelectionOnClick
					disableColumnSelector
					components={{
						Footer: () => (
							<div className='border-t w-full flex flex-nowrap items-center justify-between'>
								<GridToolbarFilterButton />
								<GridPagination />
							</div>
						)
					}}
					rows={rows}
					columns={
						[
							{
								field: "id",
								headerName: "ID",
								flex: 0.5,
								editable: false,
								sortable: false,
								filterable: false,
								valueFormatter: params =>
									params.value !== "new" ? params.value : ""
							},
							{
								field: "Nome",
								headerName: "Nome",
								flex: 1
							},
							{
								field: "Autore",
								headerName: "Autore",
								flex: 1
							},
							{
								field: "DataInserimento",
								headerName: "Data Inserimento",
								type: "date",
								flex: 1,
								valueGetter: params => ShowDate(params.value)
							},
							{
								field: "IsSemiLavorato",
								headerName: "Semilavorato",
								type: "boolean",
								flex: 0.5
							},
							{
								field: "actions",
								flex: 0.5,
								sortable: false,
								filterable: false,
								editable: false,
								align: "center",
								headerAlign: "center",
								renderHeader: (
									props: GridColumnHeaderParams
								) => (
									<div
										className='cursor-pointer bg-[#667eea] text-white shadow-lg rounded-md flex-center py-2 transition-all text-xl hover:text-2xl'
										style={{
											width:
												Math.floor(
													props.colDef.computedWidth
												) + "px"
										}}
										onClick={() => navigate("new")}
									>
										<i className='bx bx-plus ' />
									</div>
								),
								renderCell: (props: GridRenderCellParams) => (
									<>
										{/* Edit */}
										<i
											// Entro in modalità Edit
											className='bx bx-pencil my-bx text-[#667eea]'
											onClick={() => {
												navigate(`/ricette/${props.id}`, {
													state: {
														ricetta: props.row
													}
												})
											}}
										/>
										{/* Delete */}
										<i
											className='bx bx-trash my-bx text-red-500'
											onClick={() =>
												processDeleteRow(props.id)
											}
										/>
									</>
								)
							}
						] as GridColDef[]
					}
				/>
			</MyTableWrapper>
			{Alert}
		</>
	)
}
