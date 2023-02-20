import { Dispatch, SetStateAction, useContext } from "react"
import { Link, useLocation } from "react-router-dom"

import operatorePicture from "../imgs/operatore.png"
import useAuth from "package/hooks/useAuth"
import AppContext from "context/AppContext"
import InstallPWA from "./InstallPWA"

export default function SlideBar({
	hook,
	setHook
}: {
	hook: boolean
	setHook: Dispatch<SetStateAction<boolean>>
}) {
	// Auth
	const { auth, handlerLogOut } = useAuth()
	const { where } = useContext(AppContext)

	// Recupero il pathName così da colorare il Btn in cui mi trovo
	const { pathname } = useLocation()

	return (
		<div className={hook ? "sidebar open" : "sidebar"}>
			<div className='logo-details'>
				<div className='logo_name'>Bit Pastry</div>
				<i
					className={
						hook ? "bx bx-menu-alt-right" : "bx bx-menu"
					}
					id='btn'
					onClick={() => setHook(!hook)}
				/>
			</div>
			<ul className='nav-list'>
				<div>
					<Button
						className={
							pathname !== "/semi-lavorato"
								? "a activated"
								: "a"
						}
						label={
							where === "Preparazione"
								? "Preparazione Ordine"
								: "Ordini aperti"
						}
						to='/'
						icon='bx-cart-alt'
					/>

					{/* <Button
						className={
							pathname === "/semi-lavorato"
								? "a activated"
								: "a"
						}
						label='Prepara semilavorato'
						to='/semi-lavorato'
						icon='bx-layer'
					/> */}
				</div>
				<InstallPWA />
			</ul>
			<li className='profile'>
				<div className='profile-details'>
					<img
						src={operatorePicture}
						alt='Operatore'
					/>
					<div className='name_job'>
						<div className='name'>{auth.FullName}</div>
						<div className='job'>Operatore {auth.Username}</div>
					</div>
				</div>
				<i
					className='bx bx-log-out'
					id='log_out'
					onClick={handlerLogOut}
				/>
			</li>
		</div>
	)
}

/* --------------------------------------------------------------------------------------------- */
// Button
/* --------------------------------------------------------------------------------------------- */
function Button({
	className,
	to,
	label,
	icon
}: {
	className: string
	to: string
	label: string
	icon: string
}) {
	return (
		<li>
			<Link
				to={to}
				className={className}
			>
				<i className={"bx " + icon} />
				<span className='links_name'>{label}</span>
			</Link>
			<span className='tooltip'>{label}</span>
		</li>
	)
}
