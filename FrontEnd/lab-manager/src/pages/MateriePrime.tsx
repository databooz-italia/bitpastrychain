import { MateriaPrima } from "package/types"
import MyTable from "package/components/MyTable"
import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid"
import { ShowUnitàMisura, UnitàMisura } from "package/Helpers"
import { useNavigate } from "react-router-dom"

const url = "api/materie-prime"

export default function MateriePrime() {
	const navigate = useNavigate()

	/* --------------------------------------------------------------------------------------------- */
	return (
		<MyTable<MateriaPrima>
			title='Materie Prime'
			columns={
				[
					{
						field: "Nome",
						headerName: "Nome",
						flex: 1.5,
						editable: true
					},
					{
						field: "UnitàMisura",
						headerName: "Unità di Misura",
						flex: 1,
						editable: true,
						type: "singleSelect",
						valueGetter: params => params.value ?? "Kg",
						valueOptions: Object.keys(UnitàMisura),
						valueFormatter: params =>
							ShowUnitàMisura(params.value)
					},
					{
						field: "NomeRicetta",
						headerName: "Ricetta",
						flex: 1,
						editable: false,
						renderCell: (props: GridRenderCellParams) => {
							if (!props.value) return null
							return (
								<div
									className='cursor-pointer bg-[#667eea] text-white shadow-lg rounded-md flex-center py-2 transition-all text-base hover:text-lg'
									style={{
										width:
											Math.floor(
												props.colDef.computedWidth
											) + "px"
									}}
									onClick={() =>
										navigate(
											`/ricette/${
												(props.row as MateriaPrima)
													.IDRicetta
											}`
										)
									}
								>
									{props.value}
								</div>
							)
						}
					}
				] as GridColDef[]
			}
			doRenderActions={row => {
				if (!row.IDRicetta) return true
				return false
			}}
			url={url}
			firstColumn='Nome'
			deleteMessage="Confermi di voler cancellare definitivamente la riga selezionata, e l'Ingrediente da tutte le Ricette? L'azione è irreversibile."
		/>
	)
}
