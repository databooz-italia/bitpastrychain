import { Dispatch, SetStateAction, useEffect, useState } from "react"
import SearchBar from "../components/SearchBar"
import { Ordine } from "../package/types"
import useAxios from "package/hooks/useAxios"
import { ErrorHandler, ShowDate } from "package/Helpers"
import OrdiniWrapper from "components/OrdiniWrapper"
import { Where } from "context/AppContext"

export default function Ordini({
	setWhere
}: {
	setWhere: Dispatch<SetStateAction<Where>>
}) {
	const _axios = useAxios()
	const [ordini, setOrdini] = useState<Ordine[]>([])

	// Recupero di tutti gli Ordini Free
	useEffect(() => {
		const fetchData = async () => {
			// Recupero tutti gli Ordini
			const { data }: { data: Ordine[] } = await _axios.get(
				"api/ordini",
				{
					params: { onlyFree: true }
				}
			)
			setOrdini(data)
		}

		try {
			fetchData()
		} catch (e) {
			ErrorHandler(e)
		}
	}, [_axios])

	/* --------------------------------------------------------------------------------------------- */
	// Handler Prendi In Carico
	const handlePrendiCarico = async (id: number) => {
		try {
			await _axios.put(`api/presa-carico/${id}`)
			setWhere("Preparazione")
		} catch (e) {
			ErrorHandler(e)
		}
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [filter, setFilter] = useState<string>("")

	return (
		<OrdiniWrapper setWhere={setWhere}>
			<main className='w-full min-h-full bg-[#e4e5e6] p-4'>
				<header className='flex justify-between p-4'>
					<div className='w-3/12'>
						<SearchBar
							placeholder='Filtra per prodotto'
							onChange={e => setFilter(e.currentTarget.value)}
						/>
					</div>
					{/* <Link
						className='w-3/12 bg-my-green text-white my-btn flex-center'
						to='semi-lavorato'
					>
						Crea Semilavorato
					</Link> */}
				</header>
				<section className='grid 2xl:grid-cols-3 gap-12 p-12 grid-cols-2'>
					{ordini
						.filter(x =>
							x.Ricetta.Nome.toLowerCase().includes(
								filter.toLowerCase()
							)
						)
						.map(item => (
							<Card
								key={item.id}
								item={item}
								onPresaCarico={handlePrendiCarico}
							/>
						))}
				</section>
			</main>
		</OrdiniWrapper>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Card
/* --------------------------------------------------------------------------------------------- */
function Card({
	item,
	onPresaCarico
}: {
	item: Ordine
	onPresaCarico: (idOrdine: number) => void
}) {
	// Per nascondere l'Ordine
	const [isNascondi, setNascondi] = useState<boolean>(false)
	const step = item.Ricetta?.Bops?.find(
		x => !x.TsFineLavorazione
	)?.OrderIndex
	const status = step !== 0 ? "InProgress" : "NotInProgress"

	if (!isNascondi)
		return (
			<div
				className={`w-full h-full flex flex-col justify-between border border-gray-300 rounded-lg shadow-md hover:shadow-lg overflow-hidden relative ${
					status === "InProgress" ? "bg-green-100" : "bg-white"
				}`}
			>
				<header className='text-center p-8 bg-my-grigio border-b border-gray-300'>
					<h3 className='font-bold text-[2rem]'>{`Ordine #BO-${item.id} ${item.Ricetta.Nome} (${item.Quantità} ${item.Ricetta.UnitàMisura})`}</h3>
					<h3 className='font-bold text-[1.5rem] text-red-500'>
						Dati di Processo per la BitPastry Chain
					</h3>
				</header>
				<div className='flex flex-col space-y-8 font-semibold text-3xl text-center my-20 mx-10'>
					<span>{`Cliente: ${item.ContattoCliente}`}</span>
					<span>
						{`${item.Ricetta.Nome} qt: ${item.Quantità} ${item.Ricetta.UnitàMisura}`}
					</span>
					<span>
						{`Data Richiesta: ${ShowDate(item.TsOrdine)}`}
					</span>
					{status === "InProgress" ? (
						<span>
							{`Step: ${
								step === undefined
									? item.Ricetta?.Bops?.length
									: step
							}/${item.Ricetta?.Bops?.length}`}
						</span>
					) : null}
				</div>
				<footer className='p-8 bg-my-grigio flex items-center justify-between border-t border-gray-300'>
					<div
						className='bg-my-yellow text-black my-btn p-4 w-5/12'
						onClick={() => setNascondi(true)}
					>
						Non mostrare
					</div>
					<div
						className='bg-my-green text-white my-btn p-4 w-5/12'
						onClick={() => onPresaCarico(item.id as number)}
					>
						Prendi in carico
					</div>
				</footer>
			</div>
		)
	return null
}
