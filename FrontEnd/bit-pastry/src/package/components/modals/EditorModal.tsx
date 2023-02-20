import { Editor } from "@tinymce/tinymce-react";
import { Editor as TinyMCEEditor } from "tinymce";

import { Dispatch, ReactNode, SetStateAction, useRef } from "react";

export default function EditorModal({
	onSubmit,
	setModal,
	initialValue,
}: {
	onSubmit: (result: string) => void;
	setModal: Dispatch<SetStateAction<ReactNode>>;
	initialValue: string;
}) {
	// Riferimenti
	const editorRef = useRef<TinyMCEEditor | null>(null);

	/* --------------------------------------------------------------------------------------------- */
	return (
		<div
			className='relative z-10'
			aria-labelledby='modal-title'
			role='dialog'
			aria-modal='true'
		>
			<div className='fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity' />

			<div
				className='fixed inset-0 z-10 overflow-y-auto'
				onClick={() => setModal(undefined)}
			>
				<div className='flex flex-col min-h-full items-end justify-center p-4 sm:items-center sm:p-0'>
					<div className='w-5/6 shadow-xl sm:my-8'>
						<Editor
							onInit={(_, editor) => {
								editorRef.current = editor;
							}}
							initialValue={initialValue}
							apiKey='9nvb6egis494e8phah7maom0sx7rp3bm08zx2a1bd6mlf2qe'
						/>
					</div>
					<div
						className='z-50 text-center w-1/5 cursor-pointer py-2 rounded-md bg-[#f7497d] text-2xl text-white transition-all shadow-md hover:scale-105'
						onClick={e => {
							// Non faccio propagare l'evento
							e.stopPropagation();

							// Ritorno il context al chiamante
							if (editorRef.current)
								onSubmit(editorRef.current.getContent());
							setModal(undefined);
						}}
					>
						Conferma
					</div>
				</div>
			</div>
		</div>
	);
}
