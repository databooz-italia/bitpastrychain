import {
	DataGrid,
	GridPagination,
	GridRowModesModel,
	GridToolbarFilterButton,
	DataGridProps,
	GridRowId,
	GridRenderCellParams,
	GridColumnHeaderParams,
	GridRowModes
} from "@mui/x-data-grid"

import { ReactNode, useEffect, useState } from "react"
import DeleteRowModal from "./modals/DeleteRowModal"
import MyTableWrapper from "./MyTableWrapper"
import { ErrorHandler, IsEmpty } from "package/Helpers"
import useAxios from "package/hooks/useAxios"

interface MyTableProps<T>
	extends Omit<
		DataGridProps,
		| "disableDensitySelector"
		| "disableColumnMenu"
		| "disableSelectionOnClick"
		| "disableColumnSelector"
		| "editMode"
		| "page"
		| "rowModesModel"
		| "onRowEditStart"
		| "onRowEditStop"
		| "onRowModesModelChange"
		| "onProcessRowUpdateError"
		| "experimentalFeatures"
		| "components"
		| "rows"
	> {
	title: string
	url: string
	firstColumn: keyof T
	deleteMessage?: string
	doRenderActions?: (row: T) => boolean
}

/* --------------------------------------------------------------------------------------------- */
export default function MyTable<T extends { id: number | "new" }>({
	columns,
	url,
	firstColumn,
	deleteMessage,
	doRenderActions,
	...props
}: MyTableProps<T>) {
	// Gestione dello Stato
	const _axios = useAxios()
	const [rows, setRows] = useState<T[]>([])

	// Popolo i dati
	useEffect(() => {
		const fetchData = async () => {
			const result = await _axios.get(url)
			console.log(result)
			setRows(result.data as T[])
		}

		fetchData()
	}, [_axios, url])

	/* --------------------------------------------------------------------------------------------- */
	// Processo di Update / Insert
	const processRow = async (target: T) => {
		const targetID = target.id

		// IF targetID == "new"
		//    Insert
		// ELSE
		//    Update
		if (targetID === "new") {
			console.log(target)
			const result = await _axios.post(url, JSON.stringify(target))

			// Il nuovo id
			target.id = result.data
		} else {
			await _axios.put(
				`${url}/${targetID}`,
				JSON.stringify(target)
			)
		}

		// Cambio la Row all'interno dell'Hook
		setRows(prev =>
			prev.map(row => (row.id === targetID ? target : row))
		)
		return target
	}

	// Handler Eliminazione Errore
	const processDeleteRow = (id: GridRowId) => {
		setAlert(
			<DeleteRowModal
				deleteMessage={deleteMessage}
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

	// Handler new Row
	const processNewRow = () => {
		// Rest
		setPage(0)

		// Aggiungo la nuova Row ed Entro in modalità di Editing
		setRows(prev => [{ id: "new" } as T, ...prev])
		setRowModesModel({
			...rowModesModel,
			new: {
				mode: GridRowModes.Edit,
				fieldToFocus: firstColumn as string
			}
		})
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [rowModesModel, setRowModesModel] =
		useState<GridRowModesModel>({})
	const [page, setPage] = useState<number>(0)
	const [Alert, setAlert] = useState<ReactNode>(undefined)
	const isEditMode = IsEmpty(rowModesModel)

	return (
		<>
			<MyTableWrapper title={props.title}>
				<DataGrid
					{...props}
					disableDensitySelector
					disableColumnMenu
					disableSelectionOnClick
					disableColumnSelector
					editMode='row'
					page={page}
					rowModesModel={rowModesModel}
					onRowEditStart={(_params, event) =>
						(event.defaultMuiPrevented = true)
					}
					onRowEditStop={(_params, event) =>
						(event.defaultMuiPrevented = true)
					}
					onRowModesModelChange={newModel =>
						setRowModesModel(newModel)
					}
					processRowUpdate={processRow}
					onProcessRowUpdateError={ErrorHandler}
					experimentalFeatures={{ newEditingApi: true }}
					components={{
						Footer: () => (
							<div className='border-t w-full flex flex-nowrap items-center justify-between'>
								<GridToolbarFilterButton />
								<GridPagination
									page={page}
									onPageChange={(_event, newPage) =>
										setPage(newPage)
									}
									rowsPerPage={25}
								/>
							</div>
						)
					}}
					rows={rows}
					columns={[
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
						...columns,
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
									className={`cursor-pointer bg-[#667eea] text-white shadow-lg rounded-md flex-center py-2 transition-all text-xl ${
										isEditMode
											? "hover:text-2xl"
											: "pointer-events-none"
									}`}
									style={{
										width:
											Math.floor(
												props.colDef.computedWidth
											) + "px"
									}}
									onClick={processNewRow}
								>
									<i className='bx bx-plus ' />
								</div>
							),
							renderCell: (props: GridRenderCellParams) => {
								if (
									doRenderActions &&
									!doRenderActions(props.row)
								)
									return

								const _id = props.id
								if (
									rowModesModel[props.id]?.mode ===
									GridRowModes.Edit
								) {
									// Renderer Edit Mode
									return (
										<>
											{/* Save */}
											<i
												className='bx bx-save my-bx text-[#667eea]'
												onClick={() =>
													setRowModesModel({
														...rowModesModel,
														[_id]: {
															mode: GridRowModes.View
														}
													})
												}
											/>
											{/* Annulla */}
											<i
												className='bx bx-x my-bx text-red-500'
												onClick={() => {
													// Elimino dalle rows la riga "new"
													if (_id === "new")
														setRows(prev =>
															prev.filter(
																row =>
																	row.id !== "new"
															)
														)

													// Ritorno alla modalità View ignorando le modiche della "new" row
													setRowModesModel({
														...rowModesModel,
														[_id]: {
															mode: GridRowModes.View,
															ignoreModifications: true
														}
													})
												}}
											/>
										</>
									)
								}

								// Render Actions Mode
								return (
									<>
										{/* Edit */}
										<i
											// Entro in modalità Edit
											className={`bx bx-pencil my-bx text-[#667eea] ${
												isEditMode
													? ""
													: "pointer-events-none"
											}`}
											onClick={() =>
												setRowModesModel({
													...rowModesModel,
													[_id]: {
														mode: GridRowModes.Edit
													}
												})
											}
										/>
										{/* Delete */}
										<i
											className={`bx bx-trash my-bx text-red-500 ${
												isEditMode
													? ""
													: "pointer-events-none"
											}`}
											onClick={() => processDeleteRow(_id)}
										/>
									</>
								)
							}
						}
					]}
				/>
			</MyTableWrapper>
			{Alert}
		</>
	)
}
