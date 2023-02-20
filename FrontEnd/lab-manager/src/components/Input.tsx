import { HTMLInputTypeAttribute } from "react";
import { UseFormRegister } from "react-hook-form";

interface InputProps
	extends Omit<
		React.InputHTMLAttributes<HTMLInputElement> &
			React.TextareaHTMLAttributes<HTMLTextAreaElement>,
		"type" | "id"
	> {
	id: string;
	label: string;
	type?: HTMLInputTypeAttribute | "text-area";
	register?: UseFormRegister<any>;
}

export default function Input({
	label,
	type = "text",
	register,
	className,
	...props
}: InputProps) {
	const _props = {
		className: `rounded-md border-2 border-gray-300 relative block w-full appearance-none px-3 py-2 text-gray-900 placeholder-gray-500 focus:z-10 focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm ${
			className ? className : ""
		}`,
		...props,
		...(register && register(props.id)),
	};

	/* --------------------------------------------------------------------------------------------- */
	// Input / Text Area
	return (
		<div className='w-full my-4'>
			<label
				htmlFor={props.name}
				className='block text-base text-gray-900 font-semibold'
			>
				{label}
			</label>
			{/* Text Aera */}
			{type === "text-area" ? (
				<textarea {..._props} />
			) : (
				// Input Normale
				<input
					type={type}
					{..._props}
				/>
			)}
		</div>
	);
}
