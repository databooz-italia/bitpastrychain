/* --------------------------------------------------------------------------------------------- */
// Auth

import { UnitàMisura } from "./Helpers"

/* --------------------------------------------------------------------------------------------- */
export type User = {
	Username: string
	id: number
	Token: string
	FullName: string
}

/* --------------------------------------------------------------------------------------------- */
// Error
/* --------------------------------------------------------------------------------------------- */
export type ManagedError = {
	Type: string
	Status: number
	Title: string
	Detail: string
	Instance: string
	Extensions: { [key: string]: number }
}

/* --------------------------------------------------------------------------------------------- */
// Operatore
/* --------------------------------------------------------------------------------------------- */
export type Operatore = {
	id: number | "new"
	Matricola: number
	Nome: string
	Cognome: string
	Contatto: string
	DataInserimento: string //Formato yyyy-MM-dd
	Livello: number
}

/* --------------------------------------------------------------------------------------------- */
// Materia Prima
/* --------------------------------------------------------------------------------------------- */
export type MateriaPrima = {
	id: number | "new"
	Nome: string
	UnitàMisura: keyof typeof UnitàMisura
	IDRicetta: number
	NomeRicetta: string
}

/* --------------------------------------------------------------------------------------------- */
// Ricetta
/* --------------------------------------------------------------------------------------------- */
export type Ricetta = {
	id: number
	Nome: string
	Autore: string
	DataInserimento: string //Formato yyyy-MM-dd
	Descrizione: string
	IsSemiLavorato: boolean
	UnitàMisura: keyof typeof UnitàMisura
	Bops?: Bop[]
}

export type Bop = {
	id: number
	OrderIndex: number
	Titolo: string
	Descrizione: string
	Boms?: Bom[]
}

export type Bom = {
	id: number
	IDMateriaPrima: number
	Quantità: number
}

/* --------------------------------------------------------------------------------------------- */
// Ordine
/* --------------------------------------------------------------------------------------------- */
export type Ordine = {
	id: number | "new"
	Quantità: number
	TsOrdine: string
	RifCliente: string
	ContattoCliente: string
	RicettaID: number
	Ricetta: Omit<Ricetta, "Bops"> & {
		Bops: (Omit<Bop, "Boms"> & {
			IDLavorazione: number
			TsPresaInCarico: string
			TsFineLavorazione: string
			Boms: (Bom & {
				MateriaPrima: MateriaPrima
				IDRaccoltaMateriali: number
				TsPrelievo: string
				QuantitàPrelevata: number
			})[]
		})[]
	}
}
