import { SyntheticEvent, useRef } from "react"

import { User } from "../types"
import Axios from "package/apis/Axios"
import useAuth from "../hooks/useAuth"
import { ErrorHandler } from "../Helpers"

export function Login({ url }: { url: string }) {
	const { handlerLogIn } = useAuth()

	// Tutti gli Input
	const usernameRef = useRef<HTMLInputElement>(null)
	const passwordRef = useRef<HTMLInputElement>(null)
	const rememberMeRef = useRef<HTMLInputElement>(null)

	/* ------------------------------------------------------------------------------------------ */
	// Submit Handler
	const handleSubmit = async (e: SyntheticEvent) => {
		e.preventDefault()

		try {
			// Chiamata API al back-end
			const response = await Axios.post(
				url,
				JSON.stringify({
					Username: usernameRef?.current?.value,
					Password: passwordRef?.current?.value,
					IsRememberMe: rememberMeRef?.current?.checked
				}),
				{
					headers: { "Content-Type": "application/json" }
				}
			)
			const user: User = response.data

			// Eseguo il Login
			handlerLogIn(user)
		} catch (e) {
			ErrorHandler(e)
		}
	}

	/* ------------------------------------------------------------------------------------------ */
	return (
		<main className='w-full h-full flex-center bg-c'>
			<div className='w-3/12 p-4 bg-white border border-gray-200 rounded-lg sm:p-6 md:p-8 shadow-xl'>
				<form
					className='space-y-6'
					onSubmit={handleSubmit}
				>
					<h1 className='text-2xl font-medium text-gray-900 '>
						Sign in to our platform
					</h1>
					<div>
						<label
							htmlFor='username'
							className='block mb-2 text-sm font-medium text-gray-900 '
						>
							{url.includes("operatore")
								? "Matricola"
								: "Username"}
						</label>
						<input
							ref={usernameRef}
							type='username'
							autoComplete='username'
							name='username'
							id='username'
							className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:z-10 focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 block w-full p-2.5'
							placeholder={
								url.includes("operatore") ? "007" : "Admin"
							}
							required
						/>
					</div>
					<div>
						<label
							htmlFor='password'
							className='block mb-2 text-sm font-medium text-gray-900'
						>
							Your password
						</label>
						<input
							ref={passwordRef}
							type='password'
							name='password'
							id='password'
							placeholder='••••••••'
							autoComplete='current-password'
							className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:z-10 focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 block w-full p-2.5'
						/>
					</div>
					<div className='flex items-start'>
						<div className='flex items-start'>
							<div className='flex items-center h-5'>
								<input
									ref={rememberMeRef}
									id='remember'
									type='checkbox'
									className='w-4 h-4 border border-gray-300 rounded bg-gray-50 focus:ring-3 focus:ring-blue-300'
								/>
							</div>
							<label
								htmlFor='remember'
								className='ml-2 text-sm font-medium text-gray-900'
							>
								Remember me
							</label>
						</div>
					</div>
					<button
						type='submit'
						className='w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center'
					>
						Login to your account
					</button>
				</form>
			</div>
		</main>
	)
}
