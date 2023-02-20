export default function SearchBar(props: {
	placeholder: string
	onChange: React.ChangeEventHandler<HTMLInputElement>
}) {
	return (
		<div className='relative w-full group'>
			<input
				className='bg-gray-50 border border-gray-300 text-gray-900 text-base
            rounded-lg group-focus-within:outline-none group-focus-within:ring-indigo-500 group-focus-within:border-indigo-500 block w-full pr-10 p-2.5'
				type='text'
				id='search-bar'
				{...props}
			/>
			<div className='absolute inset-y-0 right-4 flex items-center pl-3 pointer-events-none text-[#9CA3AF] text-2xl group-focus-within:text-indigo-500'>
				<i className='bx bx-search-alt' />
			</div>
		</div>
	)
}
