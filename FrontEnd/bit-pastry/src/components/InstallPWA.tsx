import { useEffect, useState, SyntheticEvent } from "react"

/**
 * The BeforeInstallPromptEvent is fired at the Window.onbeforeinstallprompt handler
 * before a user is prompted to "install" a web site to a home screen on mobile.
 */
interface BeforeInstallPromptEvent extends Event {
	/**
	 * Returns an array of DOMString items containing the platforms on which the event was dispatched.
	 * This is provided for user agents that want to present a choice of versions to the user such as,
	 * for example, "web" or "play" which would allow the user to chose between a web version or
	 * an Android version.
	 */
	readonly platforms: Array<string>

	/**
	 * Returns a Promise that resolves to a DOMString containing either "accepted" or "dismissed".
	 */
	readonly userChoice: Promise<{
		outcome: "accepted" | "dismissed"
		platform: string
	}>

	/**
	 * Allows a developer to show the install prompt at a time of their own choosing.
	 * This method returns a Promise.
	 */
	prompt(): Promise<void>
}

const InstallPWA = () => {
	const [state, setState] =
		useState<BeforeInstallPromptEvent | null>(null)

	useEffect(() => {
		const ready = (e: BeforeInstallPromptEvent) => {
			e.preventDefault()
			setState(e)
		}

		// Mi serve l'Evento Before Install Prompt, ovvero l'evento che si occupa di mostrare la finestra in alto a destra del Download
		window.addEventListener("beforeinstallprompt", ready as any)

		return () => {
			window.removeEventListener(
				"beforeinstallprompt",
				ready as any
			)
		}
	}, [])

	/* --------------------------------------------------------------------------------------------- */
	// Handler On Click
	const handlerOnClick = async (e: SyntheticEvent) => {
		e.preventDefault()
		if (state) await state.prompt()
	}

	/* --------------------------------------------------------------------------------------------- */
	// Layout

	if (state)
		return (
			<li className='btn-download !fixed bottom-[70px] w-[50px] cursor-pointer'>
				<div
					className='a !bg-[#1d1b31] group '
					onClick={handlerOnClick}
				>
					<i
						className='bx bx-download !hover:text-white'
						style={{ color: "#ffff" }}
					/>
					<span
						className='links_name '
						style={{ color: "#ffff" }}
					>
						Download App
					</span>
				</div>

				<span className='tooltip'>Download App</span>
			</li>
		)
	return null
}

export default InstallPWA
