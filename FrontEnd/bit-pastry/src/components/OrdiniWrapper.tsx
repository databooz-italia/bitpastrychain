import useAxios from "package/hooks/useAxios"
import { Dispatch, SetStateAction, useEffect, useState } from "react"
import { ErrorHandler } from "package/Helpers"
import { Where } from "context/AppContext"

const url = "api/presa-carico"

export default function OrdiniWrapper({
	setWhere,
	children
}: {
	setWhere: Dispatch<SetStateAction<Where>>
	children: JSX.Element
}) {
	const _axios = useAxios()

	// Check Presa Carico
	useEffect(() => {
		const fetchData = async () => {
			const { data }: { data: boolean } = await _axios.get(url)
			if (data) setShowModal(true)
		}

		try {
			fetchData()
		} catch (e) {
			ErrorHandler(e)
		}
	}, [_axios])

	/* --------------------------------------------------------------------------------------------- */
	// Gestione del Click Yes
	const handlerYes = () => setWhere("Preparazione")

	// Gestione del Click No
	const handlerNo = async () => {
		// Chiamata API
		try {
			await _axios.delete(url)
			setShowModal(false)
		} catch (e) {
			ErrorHandler(e)
		}
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout
	const [showModal, setShowModal] = useState<boolean>(false)

	return (
		<>
			{children}
			{showModal ? (
				<PendingModal
					onYes={handlerYes}
					onNo={handlerNo}
				/>
			) : null}
		</>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Pending Modal
/* --------------------------------------------------------------------------------------------- */
const PendingModal = ({
	onYes,
	onNo
}: {
	onYes: () => void
	onNo: () => void
}) => {
	return (
		<div
			className='relative z-10'
			aria-labelledby='modal-title'
			role='dialog'
			aria-modal='true'
		>
			<div className='fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity'></div>

			<div className='fixed inset-0 z-10 overflow-y-auto'>
				<div className='flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0'>
					<div className='relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg'>
						<div className='bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4'>
							<div className='sm:flex sm:items-start'>
								<div
									className='mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full sm:mx-0 sm:h-10 sm:w-10'
									style={{
										backgroundColor:
											"rgba(238, 191, 36, 0.1)"
									}}
								>
									<svg
										className='h-6 w-6'
										style={{
											color: "rgba(238, 191, 36, 1)"
										}}
										xmlns='http://www.w3.org/2000/svg'
										fill='none'
										viewBox='0 0 24 24'
										strokeWidth='1.5'
										stroke='currentColor'
										aria-hidden='true'
									>
										<path
											strokeLinecap='round'
											strokeLinejoin='round'
											d='M12 10.5v3.75m-9.303 3.376C1.83 19.126 2.914 21 4.645 21h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 4.88c-.866-1.501-3.032-1.501-3.898 0L2.697 17.626zM12 17.25h.007v.008H12v-.008z'
										/>
									</svg>
								</div>
								<div className='mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left'>
									<h3
										className='text-lg font-medium leading-6 text-gray-900'
										id='modal-title'
									>
										Warning!
									</h3>
									<div className='mt-2'>
										<p className='text-sm text-gray-500'>
											Risulta un ordine aperto già preso in
											carico, riprenderne la lavorazione?
										</p>
									</div>
								</div>
							</div>
						</div>
						<div className='bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6'>
							<button
								type='button'
								className='inline-flex w-full justify-center rounded-md border border-transparent px-4 py-2 text-base font-medium text-white shadow-sm hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 sm:ml-3 sm:w-auto sm:text-sm'
								style={{
									color: "#fff",
									backgroundColor: "#4ebd75"
								}}
								onClick={onYes}
							>
								Si
							</button>
							<button
								type='button'
								className='mt-3 inline-flex w-full justify-center rounded-md border border-gray-300 px-4 py-2 text-base font-medium text-gray-700 shadow-sm hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm'
								style={{
									color: "#fff",
									backgroundColor: "#ffc107"
								}}
								onClick={onNo}
							>
								No
							</button>
						</div>
					</div>
				</div>
			</div>
		</div>
	)
}
