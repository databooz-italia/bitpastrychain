import MuiAccordion from "@mui/material/Accordion"
import MuiAccordionSummary from "@mui/material/AccordionSummary"
import MuiAccordionDetails from "@mui/material/AccordionDetails"
import {
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow
} from "@mui/material"

import EditorModal from "package/components/modals/EditorModal"

import { Bom, MateriaPrima, Ricetta } from "package/types"

import Input from "../components/Input"
import {
	Dispatch,
	ReactNode,
	SetStateAction,
	useEffect,
	useState
} from "react"
import {
	Control,
	Controller,
	UseFormGetValues,
	UseFormRegister,
	UseFormSetValue,
	useFieldArray,
	useForm
} from "react-hook-form"
import { UnitàMisura, ShowUnitàMisura } from "package/Helpers"
import { useParams, useLocation, useNavigate } from "react-router-dom"
import { Bop } from "../package/types"
import useAxios from "package/hooks/useAxios"
import { ShowISODate, ErrorHandler } from "../package/Helpers"

const url = "api/ricette"

export default function ModificaRicetta() {
	const { RicettaID } = useParams()
	const navigate = useNavigate()
	const { state } = useLocation()
	const _axios = useAxios()
	const {
		control,
		register,
		handleSubmit,
		reset,
		setValue,
		getValues
	} = useForm<Ricetta>()

	/* ------------------------------------------------------------------------------------------ */
	// Recupero tutte le Materie Prime
	const [materiePrime, setMateriePrime] = useState<MateriaPrima[]>(
		[]
	)

	useEffect(() => {
		const fetchData = async () => {
			// IF exist ricetta in state
			//    Load from state
			// ELSE IF RicettaID is Int
			//    API
			// ELSE IF url === "ricette/new"
			//    Nuova ricetta
			// ELSE
			//    navigate(-1)
			if (state && state.ricetta) reset(state.ricetta)
			else if (RicettaID && !isNaN(Number(RicettaID))) {
				try {
					const response = await _axios.get(url, {
						params: { id: RicettaID }
					})

					// Recupero la Ricetta dalla Response
					const _ricetta = response.data[0] as Ricetta
					if (_ricetta) reset(_ricetta)
					else navigate(-1)
				} catch {}
			} else if (RicettaID === "new") reset({})
			else navigate(-1)

			// Recupero tutte le materie prime
			const response = await _axios.get("api/materie-prime")
			setMateriePrime(response.data as MateriaPrima[])
		}

		try {
			fetchData()
		} catch {}
	}, [_axios, reset, navigate, state, RicettaID])

	/* --------------------------------------------------------------------------------------------- */
	// Handler del Submit
	const onSubmit = handleSubmit(async data => {
		try {
			console.log(data)

			await _axios.post(url, JSON.stringify(data))
			navigate(-1)
		} catch (e) {
			ErrorHandler(e)
		}
	})

	/* --------------------------------------------------------------------------------------------- */
	const [Editor, setEditor] = useState<ReactNode>(undefined)

	return (
		<>
			<form
				className='w-full flex flex-nowrap py-7 px-4'
				onSubmit={onSubmit}
			>
				<div className='w-full flex flex-col items-start p-8'>
					<div className='w-full p-5 border-y border-solid border-c'>
						<h2 className='text-[#6b7280] font-bold text-2xl'>
							{RicettaID === "new"
								? "Nuova Ricetta"
								: "Modifica Ricetta"}
						</h2>
					</div>
					<Input
						id='Nome'
						label='Nome'
						register={register}
					/>
					<Input
						id='Autore'
						label='Autore'
						register={register}
					/>
					<Input
						id='DataCreazione'
						label='Data creazione'
						disabled
						type='date'
						value={ShowISODate(getValues("DataInserimento"))}
					/>
					<Input
						id='Descrizione'
						label='Descrizione'
						type='text-area'
						rows={5}
						register={register}
					/>
					{/* Semilavorato / Unità di Misura */}
					<div className='w-full flex flex-nowrap items-center justify-between'>
						<div className='flex items-center ml-1'>
							<input
								className='h-5 w-5 rounded border-2 border-gray-100
                  text-indigo-600 focus:ring-indigo-500 cursor-pointer'
								id='IsSemiLavorato'
								type='checkbox'
								{...register("IsSemiLavorato")}
								disabled={RicettaID !== "new"}
							/>
							<label
								htmlFor='IsSemiLavorato'
								className='ml-2 block text-base font-semibold text-gray-900 cursor-pointer'
							>
								Semilavorato
							</label>
						</div>
						<div className='col-span-6 sm:col-span-3 w-3/12'>
							<label
								htmlFor='UnitàMisura'
								className='block text-base font-semibold text-gray-700'
							>
								Unità di Misura
							</label>
							<select
								className='mt-1 block w-full rounded-md border border-gray-300 bg-white py-2 px-3 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm'
								id='UnitàMisura'
								{...register("UnitàMisura")}
							>
								{Object.keys(UnitàMisura).map(x => (
									<option
										key={x}
										value={x}
									>
										{ShowUnitàMisura(
											x as keyof typeof UnitàMisura
										)}
									</option>
								))}
							</select>
						</div>
					</div>
					<input
						className='w-1/5 cursor-pointer py-2 rounded-md bg-[#f7497d] text-2xl text-white shadow-md'
						type='submit'
						value={"Salva"}
					/>
				</div>
				<Bops
					{...{
						control,
						register,
						setValue,
						getValues,
						setEditor,
						materiePrime
					}}
				/>
			</form>
			{Editor}
		</>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Bop := Step Lavorazione
/* --------------------------------------------------------------------------------------------- */
function Bops({
	control,
	register,
	setValue,
	getValues,
	setEditor,
	materiePrime
}: {
	control: Control<Ricetta, any>
	register: UseFormRegister<Ricetta>
	setValue: UseFormSetValue<Ricetta>
	getValues: UseFormGetValues<Ricetta>
	setEditor: Dispatch<SetStateAction<ReactNode>>
	materiePrime: MateriaPrima[]
}) {
	// Per gestire il Bop
	const { fields, append, remove, move } = useFieldArray({
		control,
		name: "Bops"
	})

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [expanded, setExpanded] = useState<number | null>(null)

	return (
		<div className='w-full border-l border-solid border-c p-8'>
			<div className='flex justify-end mb-4'>
				<div
					className='cursor-pointer bg-[#667eea] text-white shadow-lg rounded-md flex-center py-2 px-8 transition-all text-xl'
					onClick={() => append({} as Bop)}
				>
					Aggiungi
				</div>
			</div>
			<ul>
				{fields.map((item, index) => {
					// Registro un fake input Descrizione, che verrà settato attraverso il comando setValue
					register(`Bops.${index}.Descrizione`)

					return (
						<MuiAccordion
							key={item.id}
							className={
								expanded === item.id
									? "!border-2 !border-gray-300 !rounded !shadow-lg"
									: undefined
							}
							expanded={expanded === item.id}
							onChange={(_, expanded) =>
								setExpanded(expanded ? item.id : null)
							}
							disableGutters
						>
							<MuiAccordionSummary>
								<h3 className='text-2xl font-medium text-[#374151]'>
									{"Step lavorazione #" + (index + 1)}
								</h3>
							</MuiAccordionSummary>
							<MuiAccordionDetails>
								<div className='w-full flex justify-start -mt-4 -mb-4'>
									<div className='w-full flex-shrink-[5] m-4'>
										<label className='block text-base text-gray-900 font-semibold text-center'>
											Numero
										</label>
										<div className='flex-center py-1 px-3'>
											<i
												className='bx bxs-up-arrow text-2xl cursor-pointer'
												onClick={() => {
													if (index - 1 >= 0)
														move(index, index - 1)
												}}
											/>
											<i
												className='bx bxs-down-arrow text-2xl cursor-pointer'
												onClick={() => {
													if (index + 1 < fields.length)
														move(index, index + 1)
												}}
											/>
										</div>
									</div>
									<div className='w-16' />
									<Input
										id={`Bops.${index}.Titolo`}
										label='Titolo'
										register={register}
									/>
								</div>
								<Input
									className='cursor-pointer'
									id={"undefined"}
									label='Descrizione'
									type='text-area'
									readOnly
									rows={5}
									onClick={() =>
										setEditor(
											<EditorModal
												setModal={setEditor}
												initialValue={getValues(
													`Bops.${index}.Descrizione`
												)}
												onSubmit={result =>
													setValue(
														`Bops.${index}.Descrizione`,
														result
													)
												}
											/>
										)
									}
								/>
								<Boms
									bopIndex={index}
									{...{ control, register, materiePrime }}
								/>
								<div className='w-full flex justify-end'>
									<div
										className='cursor-pointer w-4/12 mt-12 py-2 rounded-md bg-red-500 shadow-md text-center text-white text-lg'
										onClick={() => remove(index)}
									>
										Elimina
									</div>
								</div>
							</MuiAccordionDetails>
						</MuiAccordion>
					)
				})}
			</ul>
		</div>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Bom := Ingrediente
/* --------------------------------------------------------------------------------------------- */
function Boms({
	bopIndex,
	control,
	register,
	materiePrime
}: {
	bopIndex: number
	control: Control<Ricetta, any>
	register: UseFormRegister<Ricetta>
	materiePrime: MateriaPrima[]
}) {
	// Per gestire il Bom
	const { fields, append, remove } = useFieldArray({
		control,
		name: `Bops.${bopIndex}.Boms`
	})

	/* --------------------------------------------------------------------------------------------- */
	return (
		<TableContainer component={Paper}>
			<Table>
				<TableHead>
					<TableRow>
						<TableCell width={"45%"}>
							<h2 className='text-lg font-semibold text-[#374151]'>
								Nome
							</h2>
						</TableCell>
						<TableCell width={"40%"}>
							<h2 className='text-lg font-semibold text-[#374151]'>
								Quantità
							</h2>
						</TableCell>
						<TableCell
							align='center'
							width={"15%"}
						>
							<div
								className='cursor-pointer bg-[#667eea] text-white shadow-lg rounded-md flex-center py-2 text-xl'
								onClick={() => append({} as Bom)}
							>
								<i className='bx bx-plus ' />
							</div>
						</TableCell>
					</TableRow>
				</TableHead>
				<TableBody>
					{fields.map((item, index) => {
						return (
							<TableRow key={item.id}>
								<TableCell
									component='th'
									scope='row'
									size='small'
									padding='none'
									width={"45%"}
								>
									<div className='p-2'>
										<Controller
											control={control}
											name={`Bops.${bopIndex}.Boms.${index}.IDMateriaPrima`}
											render={({
												field: {
													onChange,
													onBlur,
													value,
													ref
												}
											}) => (
												<select
													className='w-full rounded h-full border border-gray-300 bg-white py-3 px-3 
                                          shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm'
													onBlur={onBlur}
													onChange={e =>
														onChange(
															parseInt(
																e.target.value,
																10
															)
														)
													}
													value={value}
													ref={ref}
												>
													<option value={-1} />
													{materiePrime.map(x => (
														<option
															key={x.id}
															value={+x.id}
														>
															{`${x.Nome} - ${x.UnitàMisura}`}
														</option>
													))}
												</select>
											)}
										/>
									</div>
								</TableCell>
								<TableCell
									component='th'
									scope='row'
									size='small'
									padding='none'
									width={"40%"}
								>
									<div className='p-2'>
										<Controller
											control={control}
											name={`Bops.${bopIndex}.Boms.${index}.Quantità`}
											render={({
												field: {
													onChange,
													onBlur,
													value,
													ref
												}
											}) => (
												<input
													className='w-full h-full rounded border border-gray-300 bg-white py-3 px-3 outline-none shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm'
													step='any'
													type='number'
													min='0'
													onBlur={onBlur}
													onChange={e =>
														onChange(
															parseFloat(
																e.target.value
															)
														)
													}
													defaultValue={value}
													ref={ref}
												/>
											)}
										/>
									</div>
								</TableCell>
								<TableCell
									component='th'
									scope='row'
									align='center'
									size='small'
									padding='none'
									width={"15%"}
								>
									<i
										className='bx bx-trash my-bx text-red-500'
										onClick={() => remove(index)}
									/>
								</TableCell>
							</TableRow>
						)
					})}
				</TableBody>
			</Table>
		</TableContainer>
	)
}
