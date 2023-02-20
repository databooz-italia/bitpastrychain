import MuiAccordion from "@mui/material/Accordion"
import MuiAccordionSummary from "@mui/material/AccordionSummary"
import MuiAccordionDetails from "@mui/material/AccordionDetails"

import { Ordine } from "package/types"
import {
	useEffect,
	useState,
	ReactNode,
	SetStateAction,
	Dispatch
} from "react"
import { ErrorHandler } from "package/Helpers"
import useAxios from "package/hooks/useAxios"
import { AxiosInstance } from "axios"
import PrelievoModal from "../components/PrelievoModal"
import { ShowDate } from "../package/Helpers"
import { Where } from "context/AppContext"

const url = "api/ordini/complete"

export default function Preparazione({
	setWhere
}: {
	setWhere: Dispatch<SetStateAction<Where>>
}) {
	const _axios = useAxios()
	const [ordine, setOrdine] = useState<Ordine>({} as Ordine)
	// Il bop che in questo momento è InProgress
	const boopFocused = ordine.Ricetta?.Bops?.find(
		x => !x.TsFineLavorazione
	)?.OrderIndex
	const status =
		boopFocused === undefined ? "Completato" : "InProgress"

	// Recupero l'Ordine in Preparazione
	useEffect(() => {
		const fetchData = async () => {
			const { data } = await _axios.get("api/ordini", {
				params: { onlyMy: true }
			})
			console.log(data[0])
			setOrdine(data[0] as Ordine)
		}

		try {
			fetchData()
		} catch (e) {
			ErrorHandler(e)
		}
	}, [_axios])

	/* --------------------------------------------------------------------------------------------- */
	// Handler Abort
	const handlerAbort = async () => {
		try {
			await _axios.delete("api/presa-carico")
			setWhere("Ordini")
		} catch (e) {
			ErrorHandler(e)
		}
	}

	// Handler Submit
	const handlerSubmit = async () => {
		try {
			await _axios.put(url)
			setWhere("Ordini")
		} catch (e) {
			ErrorHandler(e)
		}
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [expanded, setExpanded] = useState<number | null>(null)
	const [Modal, setModal] = useState<ReactNode>(undefined) // Prelievo Modal

	return (
		<>
			<main className='w-full min-h-full bg-[#e4e5e6] px-4 py-8'>
				<h1 className='text-center text-4xl font-bold'>{`Preparazione Ordine #BO-${
					ordine?.id ?? ""
				} ${ordine?.Ricetta?.Nome ?? ""} (${
					ordine?.Quantità ?? ""
				})`}</h1>
				<ul className='mt-10 mb-14'>
					{ordine.Ricetta?.Bops?.map((boop, boopIndex) => (
						<Boop
							_axios={_axios}
							key={boop.id}
							item={boop}
							index={boopIndex}
							expanded={expanded === boopIndex}
							boopFocused={
								boopFocused === undefined ? -1 : boopFocused
							}
							onExpand={value => setExpanded(value)}
							setModal={setModal}
							setOrdine={setOrdine}
						/>
					))}
				</ul>
				{status === "InProgress" ? (
					<div
						className='my-btn w-full py-4 text-white text-xl bg-red-500'
						onClick={handlerAbort}
					>
						Abbandona preparazione
					</div>
				) : (
					<div
						className='my-btn w-full py-4 text-white text-xl bg-my-green'
						onClick={handlerSubmit}
					>
						Completa Ordine
					</div>
				)}
			</main>
			{Modal}
		</>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Boop
/* --------------------------------------------------------------------------------------------- */
export type BoopType = Ordine["Ricetta"]["Bops"][number]

function Boop({
	_axios,
	item,
	index,
	expanded,
	boopFocused,
	onExpand,
	setModal,
	setOrdine
}: {
	_axios: AxiosInstance
	item: BoopType
	index: number
	expanded: boolean
	boopFocused: number
	onExpand: (value: number | null) => void
	setModal: Dispatch<SetStateAction<ReactNode>>
	setOrdine: Dispatch<SetStateAction<Ordine>>
}) {
	/* --------------------------------------------------------------------------------------------- */
	/*
      IF(boopFocused === item.OrderIndex)
         InProgress
      ELSE
         IF(TSFineLavorazione exist)
            Completato
         ELSE 
            Open
   */
	// Status del Boop
	const status =
		boopFocused === item.OrderIndex
			? "InProgress"
			: item.TsFineLavorazione
			? "Completato"
			: "Open"

	/* --------------------------------------------------------------------------------------------- */
	// Handler Submit
	const handlerSubmit = async () => {
		try {
			await _axios.put(`${url}/${item.id}`)

			// Aggiorno lo Stato in modo Fake
			setOrdine(prev => {
				const _bops = prev.Ricetta.Bops[index]
				if (_bops) {
					_bops.IDLavorazione = -1
					_bops.TsFineLavorazione = ShowDate(undefined)

					// Aggiorno lo Stato del Boop Successivo im modo Fake
					if (index + 1 < prev.Ricetta.Bops.length) {
						const _nextBop = prev.Ricetta.Bops[index + 1]
						if (_nextBop) _nextBop.IDLavorazione = -1
					}
				}
				return prev
			})
			onExpand(index + 1)
		} catch (e) {
			ErrorHandler(e)
		}
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	return (
		<MuiAccordion
			className='!my-8 !border-2 !shadow !border-gray-300 !rounded !overflow-hidden'
			key={item.id}
			expanded={expanded}
			onChange={(_, expanded) => onExpand(expanded ? index : null)}
			disableGutters
		>
			<MuiAccordionSummary className='!bg-my-grigio'>
				<h3
					className={`w-full text-center text-2xl font-medium ${
						status === "Completato"
							? "text-my-green"
							: "text-my-cyan"
					}`}
				>{`# ${index + 1} - ${item.Titolo}`}</h3>
			</MuiAccordionSummary>
			<MuiAccordionDetails>
				<div className='bg-white w-full flex p-12'>
					<div className='w-5/12 text-center'>
						<h4 className='text-3xl font-semibold'>
							Ingredienti
						</h4>
						<ul>
							{item.Boms?.map((boom, boomIndex) => (
								<Boom
									_axios={_axios}
									key={boom.id}
									item={boom}
									parentID={item.id}
									index={boomIndex}
									setModal={setModal}
									setOrdine={setOrdine}
								/>
							))}
						</ul>
						<h4 className='text-3xl font-semibold mt-14'>
							Informazioni
						</h4>
						<ul></ul>
					</div>
					<div className='w-16' />
					<div className='w-full'>
						<div className='w-full bg-white border border-gray-300 rounded overflow-hidden mt-[4.25rem]'>
							<header className='text-center p-5 bg-my-grigio border-b border-gray-300'>
								<h3 className='font-bold text-2xl'>{`Descrizione Step ${item.Titolo}`}</h3>
							</header>
							<div
								className='h-full py-8 px-14 flex flex-col'
								dangerouslySetInnerHTML={{
									__html: item.Descrizione
								}}
							/>
							<footer className='py-4 px-14 bg-my-grigio border-t border-gray-300 flex justify-end'>
								{status === "InProgress" && (
									<div
										className='my-btn w-4/12 py-4 text-white text-xl bg-my-green'
										onClick={handlerSubmit}
									>
										Completa Step
									</div>
								)}
							</footer>
						</div>
					</div>
				</div>
			</MuiAccordionDetails>
		</MuiAccordion>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Boom
/* --------------------------------------------------------------------------------------------- */
export type BoomType = BoopType["Boms"][number]

function Boom({
	_axios,
	item,
	parentID,
	index,
	setModal,
	setOrdine
}: {
	_axios: AxiosInstance
	item: BoomType
	parentID: number
	index: number
	setModal: Dispatch<SetStateAction<ReactNode>>
	setOrdine: Dispatch<SetStateAction<Ordine>>
}) {
	/* --------------------------------------------------------------------------------------------- */
	/*
      IF(Ts Prelievo exist)
         Prelevato
      ELSE
         Not Prelevato
   */
	// Status del Boop
	const status: "Prelevato" | "NotPrelevato" = item.TsPrelievo
		? "Prelevato"
		: "NotPrelevato"

	/* --------------------------------------------------------------------------------------------- */
	// Handler Submit
	const handlerSubmit = () => {
		setModal(
			<PrelievoModal
				boom={item}
				setModal={setModal}
				onSubmit={async quantitàPrelevata => {
					try {
						const { data } = await _axios.post(
							`${url}/${parentID}/${item.id}`,
							JSON.stringify({
								QuantitàPrelevata: quantitàPrelevata
							})
						)

						// Aggiorno lo stato
						setOrdine((prev: Ordine) => {
							const _boop = prev.Ricetta.Bops.find(
								x => x.id === parentID
							)
							if (_boop) {
								const _boom = _boop.Boms.find(
									x => x.id === item.id
								)
								if (_boom !== undefined) {
									Object.assign(
										_boom,
										data as {
											id: number
											TsPrelievo: string
											Quantità: number
										}
									)
								}
							}
							return prev
						})

						// Chiudo il Modal che Preleva la Quantità
						setModal(undefined)
					} catch (e) {
						ErrorHandler(e)
					}
				}}
			/>
		)
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	return (
		<div
			className='w-full bg-my-grigio border border-gray-300 rounded overflow-hidden py-4 my-8 flex-center relative cursor-pointer'
			onClick={
				status === "NotPrelevato" ? handlerSubmit : undefined
			}
		>
			<span className='text-lg text-center text-my-cyan mx-16'>{`${item.MateriaPrima?.Nome} (${item.Quantità} ${item.MateriaPrima?.UnitàMisura})`}</span>
			{status === "Prelevato" && (
				<i className='bx bxs-check-circle text-my-green absolute right-8 text-4xl ' />
			)}
		</div>
	)
}
