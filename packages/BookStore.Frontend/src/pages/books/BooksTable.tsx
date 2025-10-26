import {createColumnHelper, flexRender, getCoreRowModel, useReactTable} from "@tanstack/react-table";
import {useVM} from "@/lib/helpers.ts";
import type {BooksViewModel} from "./BooksViewModel.ts";
import {useStore} from "zustand/react";

type Book = {
	id: string
	name: string
	releaseDate: string
}

const columnHelper = createColumnHelper<Book>()

const columns = [
	columnHelper.accessor('id', {
		header: 'ID',
		footer: info => info.column.id,
	}),
	columnHelper.accessor('name', {
		header: 'Name',
		footer: info => info.column.id,
	}),
	columnHelper.accessor('releaseDate', {
		header: 'Release Data',
		footer: info => info.column.id,
	}),
]

export const BooksTable = () => {
	const booksVM: BooksViewModel = useVM("booksPageViewModel") as BooksViewModel
	
	const books = useStore(booksVM.books)
	
	const table = useReactTable({
		data: books.books,
		columns,
		getCoreRowModel: getCoreRowModel(),
	})

	return (
		<table>
			<thead>
			{table.getHeaderGroups().map(headerGroup => (
				<tr key={headerGroup.id}>
					{headerGroup.headers.map(header => (
						<th key={header.id}>
							{header.isPlaceholder
								? null
								: flexRender(
									header.column.columnDef.header,
									header.getContext()
								)}
						</th>
					))}
				</tr>
			))}
			</thead>
			<tbody>
			{table.getRowModel().rows.map(row => (
				<tr key={row.id}>
					{row.getVisibleCells().map(cell => (
						<td key={cell.id}>
							{flexRender(cell.column.columnDef.cell, cell.getContext())}
						</td>
					))}
				</tr>
			))}
			</tbody>
		</table>
	);
};
