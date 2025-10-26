import {useContext, useRef} from "react";
import {Services, type ServicesT} from "../App";
import type {AwilixContainer} from "awilix";


export const useVM = (name: string) => {
	const container = useContext(Services)
	
	return container.resolve(name);
	
}
