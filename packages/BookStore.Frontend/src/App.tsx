import {RouterProvider} from "@tanstack/react-router";
import "7.css/dist/7.css";
import "@/styles/reset.css";
import {asClass, asValue, type AwilixContainer, createContainer} from "awilix";
import {BooksService} from "./services/BooksService.ts";
import {BooksViewModel} from "./pages/books/BooksViewModel.ts";
import {createContext} from "react";
import {router} from "@/routes.tsx";
import { QueryClient } from "@tanstack/react-query";
import {createStore, Provider} from "jotai";
import type {Store} from "jotai/vanilla/store";
import {DevTools} from "jotai-devtools";
import createClient from "openapi-fetch";
import type {paths} from "@book-store/frontend_api-client/src/schema";

const apiClient = createClient<paths>({ baseUrl: "https://myapi.dev/v1/" });

const queryClient = new QueryClient({
	defaultOptions: {
		queries: {
			staleTime: Infinity,
		},
	},
})

// This code is only for TypeScript
declare global {
	interface Window {
		__TANSTACK_QUERY_CLIENT__:
			import("@tanstack/query-core").QueryClient;
	}
}

// This code is for all users
window.__TANSTACK_QUERY_CLIENT__ = queryClient;

export type ServicesT = {
	store: Store
	apiClient: typeof apiClient
	booksService: BooksService
	booksPageViewModel: BooksViewModel,
	queryClient: QueryClient
}
export const Services = createContext<AwilixContainer<ServicesT>>(null!)

function App() {
	const store = createStore()
	
	const container = createContainer<ServicesT>({})
	container.register({
		store: asValue(store),
		apiClient: asValue(apiClient),
		booksService: asClass(BooksService).singleton(),
		booksPageViewModel: asClass(BooksViewModel).scoped(),
		queryClient: asValue(queryClient),
	})
	
  return (
    <>
			<DevTools store={store} />
		<Services.Provider value={container}>
			<Provider store={store}>
				<RouterProvider router={router} />
			</Provider>
		</Services.Provider>
    </>
  )
}

export default App
