import {useForm} from "@tanstack/react-form";
import {useVM} from "@/lib/helpers.ts";
import type {BooksViewModel} from "./BooksViewModel.ts";

export const AddBookForm = () => {
	const booksVM: BooksViewModel = useVM("booksPageViewModel") as BooksViewModel;

	const form = useForm({
		defaultValues: {
			name: '',
			releaseDate: '',
		},
		onSubmit: async ({value}) => {
			await booksVM.addBook(value.name, value.releaseDate);
			// Reset form after successful submission
			form.reset();
		},
	});

	return (
		<div>
			<h2>Add New Book</h2>
			<form
				onSubmit={(e) => {
					e.preventDefault();
					e.stopPropagation();
					form.handleSubmit();
				}}
			>
				<div>
					<form.Field
						name="name"
						validators={{
							onChange: ({value}) =>
								!value || value.length < 1
									? 'Book name is required'
									: undefined,
						}}
						children={(field) => (
							<>
								<label htmlFor={field.name}>Book Name:</label>
								<input
									id={field.name}
									name={field.name}
									value={field.state.value}
									onBlur={field.handleBlur}
									onChange={(e) => field.handleChange(e.target.value)}
								/>
								{field.state.meta.errors ? (
									<div style={{color: 'red'}}>
										{field.state.meta.errors.join(', ')}
									</div>
								) : null}
							</>
						)}
					/>
				</div>

				<div>
					<form.Field
						name="releaseDate"
						validators={{
							onChange: ({value}) =>
								!value || value.length < 1
									? 'Release date is required'
									: undefined,
						}}
						children={(field) => (
							<>
								<label htmlFor={field.name}>Release Date:</label>
								<input
									id={field.name}
									name={field.name}
									type="date"
									value={field.state.value}
									onBlur={field.handleBlur}
									onChange={(e) => field.handleChange(e.target.value)}
								/>
								{field.state.meta.errors ? (
									<div style={{color: 'red'}}>
										{field.state.meta.errors.join(', ')}
									</div>
								) : null}
							</>
						)}
					/>
				</div>

				<form.Subscribe
					selector={(state) => [state.canSubmit, state.isSubmitting]}
					children={([canSubmit, isSubmitting]) => (
						<button type="submit" disabled={!canSubmit}>
							{isSubmitting ? 'Adding...' : 'Add Book'}
						</button>
					)}
				/>
			</form>
		</div>
	);
};