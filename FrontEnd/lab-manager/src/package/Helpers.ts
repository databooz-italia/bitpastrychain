import { ManagedError } from "./types"
import { AxiosError } from "axios"

/* --------------------------------------------------------------------------------------------- */
export function ShowDate(value?: string): string {
	let today = new Date()
	try {
		if (value !== null && value !== undefined && value !== "")
			today = new Date(value)
	} catch {}

	const yyyy = today.getFullYear()
	let mm: number | string = today.getMonth() + 1 // Months start at 0!
	let dd: number | string = today.getDate()

	if (dd < 10) dd = "0" + dd
	if (mm < 10) mm = "0" + mm
	return dd + "/" + mm + "/" + yyyy
}

export function ShowISODate(value: string): string {
	let today = new Date()
	try {
		if (value !== null && value !== undefined && value !== "")
			today = new Date(value)
	} catch {}

	return today.toISOString().slice(0, 10)
}

/* --------------------------------------------------------------------------------------------- */
// Unità di Misura
/* --------------------------------------------------------------------------------------------- */
export enum UnitàMisura {
	Kg = "Kilogrammi",
	Pz = "Pezzi",
	L = "Litri"
}
export const ShowUnitàMisura = (
	key: keyof typeof UnitàMisura
): string => {
	return UnitàMisura[key]
}

/* --------------------------------------------------------------------------------------------- */
// Handler di Errori
/* --------------------------------------------------------------------------------------------- */
export const ErrorHandler = (e: unknown) => {
	try {
		let error = {} as ManagedError

		// Recupero il Managed Error
		if (e instanceof AxiosError && e.response && e.response.data)
			error = e.response.data

		console.error(error)
		alert(
			`${error?.Title ?? "An error occurred."}\n${
				error?.Detail ?? ""
			}`
		)
	} catch {}
}

/* --------------------------------------------------------------------------------------------- */
// Handler di Errori
/* --------------------------------------------------------------------------------------------- */
export function IsEmpty(obj: any): boolean {
	for (var prop in obj) {
		if (Object.prototype.hasOwnProperty.call(obj, prop)) {
			return false
		}
	}

	return JSON.stringify(obj) === JSON.stringify({})
}
