export default function MyTableWrapper({
	title,
	children,
}: {
	title: string;
	children: JSX.Element;
}) {
	return (
		<div className='p-4 w-full h-full min-h-full max-h-full bg-[#edf1f7]'>
			<div className='bg-white w-full h-full min-h-full rounded-xl shadow-xl flex flex-col overflow-hidden'>
				<div className='w-full p-5 border-b border-solid border-c'>
					<h2 className='text-[#6b7280] font-bold text-2xl'>
						{title}
					</h2>
				</div>
				<div className='w-full h-full p-4 flex'>{children}</div>
			</div>
		</div>
	);
}
