import { useState, useEffect } from 'react'
import { Accordion, AccordionDetails, AccordionSummary, Box, Paper } from '@mui/material'
import { CircularProgress } from '@mui/material';
import { getCalculations, getCalculationResult, Calculation } from './store';

function App() {
	const [calculations, setCalculations] = useState<Calculation[] | null>(null);


	useEffect(() => {
		const getCalculationsAsync = async () => {
			setCalculations(await getCalculations());
		}
		getCalculationsAsync();
	}, [getCalculations]);


	return <Box sx={{ height: '100vh', width: '100vw', display: 'flex', justifyContent: 'center', alignItems: 'center', }}>
		<Paper sx={{ width: '50%', padding: 2, height: '95%', margin: 'auto', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
			{calculations === null ? <CircularProgress /> : null}
			<Accordion>
				<AccordionSummary>

				</AccordionSummary>
				<AccordionDetails>

				</AccordionDetails>
			</Accordion>
		</Paper>
	</Box>
}

export default App
