import { BoomType } from "pages/Preparazione"
import {
	createRef,
	Dispatch,
	ReactNode,
	SetStateAction,
	useState
} from "react"

export default function PrelievoModal({
	boom,
	setModal,
	onSubmit
}: {
	boom: BoomType
	setModal: Dispatch<SetStateAction<ReactNode>>
	onSubmit: (quantitàPrelevata: number) => void
}) {
	// Reference all'Input di Quantità
	const inputRef = createRef<HTMLInputElement>()

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [isFirstPhase, setFirstPhase] = useState<boolean>(true)

	return (
		<div
			className='relative z-10'
			aria-labelledby='modal-title'
			role='dialog'
			aria-modal='true'
		>
			<div className='fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity' />

			<div className='fixed inset-0 z-10 overflow-y-auto'>
				<div className='flex min-h-full flex-col items-end justify-center p-4 text-center sm:items-center sm:p-0'>
					<div className='w-8/12 transform overflow-hidden bg-white border border-gray-300 rounded-lg shadow-xl'>
						<header className='text-center p-8 bg-my-grigio border-b border-gray-300'>
							<h3 className='font-bold text-4xl'>
								{`Prelievo ${boom.MateriaPrima?.Nome} ${boom.Quantità} ${boom.MateriaPrima?.UnitàMisura}`}
							</h3>
						</header>
						<div className='h-full py-8 px-14 flex flex-col'>
							{isFirstPhase ? (
								<>
									<h3 className='font-semibold text-lg'>
										Clicca sull'immagine per scansionare il
										QR Code dell'Ingrediente
									</h3>
									<div className='flex-center'>
										<svg
											version='1.1'
											width='368'
											height='368'
										>
											<defs id='defs9306' />
											<path
												d='m 16,16 0,16 0,16 0,16 0,16 0,16 0,16 0,16 16,0 16,0 16,0 16,0 16,0 16,0 16,0 0,-16 0,-16 0,-16 0,-16 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 -16,0 -16,0 -16,0 -16,0 z m 128,0 0,16 0,16 16,0 0,-16 16,0 0,-16 -16,0 -16,0 z m 32,16 0,16 16,0 0,-16 -16,0 z m 16,16 0,16 16,0 16,0 0,-16 0,-16 0,-16 -16,0 0,16 0,16 -16,0 z m 0,16 -16,0 -16,0 -16,0 0,16 16,0 16,0 0,16 -16,0 0,16 16,0 0,16 16,0 0,-16 16,0 0,16 -16,0 0,16 -16,0 0,16 16,0 16,0 0,16 16,0 0,-16 0,-16 0,-16 0,-16 0,-16 -16,0 0,-16 -16,0 0,-16 z m 16,112 -16,0 0,16 -16,0 0,16 0,16 16,0 0,16 0,16 -16,0 -16,0 0,-16 16,0 0,-16 -16,0 0,-16 0,-16 -16,0 0,-16 16,0 0,16 16,0 0,-16 0,-16 -16,0 -16,0 0,-16 -16,0 -16,0 -16,0 0,16 -16,0 0,16 -16,0 0,-16 16,0 0,-16 -16,0 -16,0 0,16 -16,0 0,16 0,16 0,16 16,0 0,-16 16,0 16,0 16,0 0,-16 16,0 0,-16 16,0 0,16 -16,0 0,16 16,0 0,16 -16,0 0,16 16,0 16,0 0,16 0,16 0,16 16,0 0,16 16,0 16,0 16,0 0,16 -16,0 -16,0 -16,0 0,-16 -16,0 0,16 0,16 16,0 0,16 -16,0 0,16 16,0 16,0 0,-16 16,0 0,16 16,0 16,0 16,0 16,0 0,-16 16,0 0,16 16,0 16,0 16,0 0,-16 -16,0 -16,0 0,-16 -16,0 0,-16 -16,0 0,16 -16,0 -16,0 0,16 -16,0 0,-16 16,0 0,-16 0,-16 0,-16 16,0 0,-16 -16,0 -16,0 0,-16 16,0 0,-16 0,-16 0,-16 -16,0 0,-16 z m 48,128 0,-16 -16,0 0,16 16,0 z m 32,16 16,0 16,0 0,-16 -16,0 -16,0 0,16 z m 32,-16 16,0 0,-16 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 0,-16 -16,0 0,16 -16,0 0,16 0,16 16,0 0,-16 16,0 0,16 0,16 16,0 16,0 0,16 z m -48,-80 0,-16 -16,0 -16,0 0,16 16,0 16,0 z m 16,0 16,0 0,-16 0,-16 0,-16 16,0 0,16 16,0 0,16 16,0 0,-16 0,-16 -16,0 0,-16 16,0 0,-16 -16,0 -16,0 0,16 -16,0 0,-16 -16,0 0,16 -16,0 0,16 16,0 0,16 0,16 0,16 z m -16,-48 -16,0 0,16 16,0 0,-16 z m 64,32 -16,0 0,16 16,0 0,-16 z m -224,0 0,-16 -16,0 0,16 16,0 z m -16,0 -16,0 -16,0 -16,0 0,16 16,0 16,0 16,0 0,-16 z m -64,0 -16,0 0,16 16,0 0,-16 z m 0,-48 0,-16 -16,0 0,16 16,0 z m 112,-16 16,0 0,-16 0,-16 -16,0 0,16 0,16 z m 96,-128 0,16 0,16 0,16 0,16 0,16 0,16 0,16 16,0 16,0 16,0 16,0 16,0 16,0 16,0 0,-16 0,-16 0,-16 0,-16 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 -16,0 -16,0 -16,0 -16,0 z m -208,16 16,0 16,0 16,0 16,0 16,0 0,16 0,16 0,16 0,16 0,16 -16,0 -16,0 -16,0 -16,0 -16,0 0,-16 0,-16 0,-16 0,-16 0,-16 z m 224,0 16,0 16,0 16,0 16,0 16,0 0,16 0,16 0,16 0,16 0,16 -16,0 -16,0 -16,0 -16,0 -16,0 0,-16 0,-16 0,-16 0,-16 0,-16 z m -208,16 0,16 0,16 0,16 16,0 16,0 16,0 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 z m 224,0 0,16 0,16 0,16 16,0 16,0 16,0 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 z m -32,96 0,16 16,0 0,-16 -16,0 z m -224,96 0,16 0,16 0,16 0,16 0,16 0,16 0,16 16,0 16,0 16,0 16,0 16,0 16,0 16,0 0,-16 0,-16 0,-16 0,-16 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 -16,0 -16,0 -16,0 -16,0 z m 16,16 16,0 16,0 16,0 16,0 16,0 0,16 0,16 0,16 0,16 0,16 -16,0 -16,0 -16,0 -16,0 -16,0 0,-16 0,-16 0,-16 0,-16 0,-16 z m 16,16 0,16 0,16 0,16 16,0 16,0 16,0 0,-16 0,-16 0,-16 -16,0 -16,0 -16,0 z m 288,48 0,16 16,0 0,-16 -16,0 z'
												id='path3093'
											/>
										</svg>
									</div>
								</>
							) : (
								<>
									<div className='flex-center flex-nowrap text-4xl font-semibold text-black mt-14 mb-20'>
										<input
											className='pt-1 text-5xl w-[12.5rem] text-right appearance-none placeholder:text-black placeholder:font-bold mr-2 focus:outline-none'
											type='number'
											min='0'
											step='any'
											placeholder='##.####'
											ref={inputRef}
										/>
										<div>{`${boom.MateriaPrima?.UnitàMisura} - Attesi ${boom.Quantità} ${boom.MateriaPrima?.UnitàMisura}`}</div>
									</div>
									<span className='font-xl font-semibold'>
										Clicca sui cancelletti per inserire
										manualmente
									</span>
								</>
							)}
						</div>
						<footer className='p-8 bg-my-grigio flex items-center justify-between border-t border-gray-300'>
							<div
								className='my-btn text-black bg-my-yellow py-3 w-4/12'
								onClick={() => setModal(undefined)}
							>
								Annulla prelievo
							</div>
							<div className='my-btn text-white bg-blue-400 py-3 w-3/12'>
								Acquisisci in Automatico
							</div>
							<div
								className='my-btn text-white bg-my-green py-3 w-4/12'
								onClick={
									isFirstPhase
										? () => setFirstPhase(false)
										: () =>
												onSubmit(
													+(
														inputRef.current?.value ||
														-1
													)
												)
								}
							>
								{isFirstPhase
									? "Confirm"
									: "Torna alla preparazione"}
							</div>
						</footer>
					</div>
				</div>
			</div>
		</div>
	)
}
