import { useState, useEffect, useCallback } from 'react'
import { Accordion, AccordionDetails, AccordionSummary, Box, Paper, Typography, TextField, Button, Snackbar } from '@mui/material'
import { CircularProgress } from '@mui/material';
import { getCalculations, getCalculationResult, Calculation } from './store';
import { ExpandMoreOutlined, Pattern } from '@mui/icons-material';

function App() {
	const [open, setOpen] = useState(false);
	const [calculations, setCalculations] = useState<Calculation[] | null>(null);
	const [textBoxValues, setTextBoxValues] = useState({});
	const [toastMessage, setToastMessage] = useState('');

	useEffect(() => {
		const getCalculationsAsync = async () => {
			setCalculations(await getCalculations());
		}
		getCalculationsAsync();
	}, [getCalculations]);

	const handleClose = useCallback(() => {

	}, [])

	return <Box sx={{ height: '100vh', width: '100vw', display: 'flex', justifyContent: 'center', alignItems: 'center', }}>
		<Paper sx={{ width: '50%', padding: 2, height: '95%', margin: 'auto', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
			{calculations === null ? <CircularProgress /> : null}
			<Box>
				{calculations?.map((x, i) => AccordionItem(x, textBoxValues, setTextBoxValues, setToastMessage, setOpen, i))}
			</Box>
		</Paper>
		<Snackbar
			open={open}
			onClose={handleClose}
			message={toastMessage}
			anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
			autoHideDuration={3000}
		/>

	</Box>
}

function AccordionItem(calculation: Calculation, textBoxValues, setTextBoxValues, setToastMessage, setOpen, index) {
	return <Accordion key={'accord' + index}>
		<AccordionSummary expandIcon={<ExpandMoreOutlined />} key={'accordSum' + index}>
			<Typography>{calculation.name}</Typography>
		</AccordionSummary>
		<AccordionDetails key={'accordDet' + index}>
			{Object.values(calculation.calculationParameters).map((x, i) => {
				return <Box key={'box' + i}>
					<Typography key={'typog' + i}>{x.name}</Typography>
					<TextField key={'txt' + i} type='number' onChange={(e) => {
						let key = calculation.name + '-' + x.name;
						let newObj = { ...textBoxValues };
						newObj[key] = e.target.value;
						setTextBoxValues(newObj);
					}}
						value={textBoxValues[calculation.name + '-' + x.name]}
						slotProps={{ htmlInput: { inputMode: 'numeric', pattern: '[0-9]*', min: x.minValue, max: x.maxValue } }} />
				</Box>
			})}
			<Button onClick={async (e) => {
				let vals = Object.values(calculation.calculationParameters)
				let qryParams = {};
				for (let i = 0; i < vals.length; i++) {
					let val = vals[i];
					qryParams[val.name] = textBoxValues[calculation.name + '-' + val.name];
				}
				let body = {
					calculationName: calculation.name,
					queryParameters: qryParams
				};
				let result = await getCalculationResult(body);
				if (result.success) {
					setToastMessage(`Success! Result is: ${result.result}`);
				}
				else {
					setToastMessage(`Error! Msg: ${result.message}`);
				}
				setOpen(true);
			}}>Submit</Button>
		</AccordionDetails>
	</Accordion>
}

export default App
