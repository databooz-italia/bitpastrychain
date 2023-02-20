import { Operatore } from "package/types"
import MyTable from "package/components/MyTable"
import { GridColDef } from "@mui/x-data-grid"
import { ShowDate } from "package/Helpers"

export default function Operatori() {
	return (
		<MyTable<Operatore>
			title='Operatori'
			columns={
				[
					{
						field: "Matricola",
						headerName: "Matricola",
						type: "number",
						headerAlign: "left",
						align: "left",
						flex: 0.5,
						editable: true
					},
					{
						field: "Nome",
						headerName: "Nome",
						flex: 1,
						editable: true
					},
					{
						field: "Cognome",
						headerName: "Cognome",
						flex: 1,
						editable: true
					},
					{
						field: "Contatto",
						headerName: "Contatto",
						flex: 1,
						editable: true
					},
					{
						field: "DataInserimento",
						headerName: "Data Inserimento",
						type: "date",
						flex: 1,
						editable: false,
						valueFormatter: params => ShowDate(params.value)
					},
					{
						field: "Livello",
						headerName: "Livello/Titolo",
						type: "number",
						headerAlign: "left",
						align: "left",
						flex: 0.5,
						editable: true,
						valueFormatter: params => params.value || 0
					}
				] as GridColDef[]
			}
			url='api/operatori'
			firstColumn='Matricola'
		/>
	)
}
