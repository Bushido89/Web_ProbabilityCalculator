
export type CalculationResultReq = {
	calculationName: string;
	queryParameters: { [key: string]: number };
}

export type CalculationResult = {
	success: boolean;
	code: number;
	result: number;
	message: string;
}

export type CalculationParameter = {
	name: string;
	minValue: number;
	maxValue: number;
}

export type Calculation = {
	name: string;
	calculationParameters: { [key: string]: CalculationParameter };

}

const API_BASE = "/api/calculation";

export async function getCalculationResult(req: CalculationResultReq): Promise<CalculationResult> {
	return await apiPost(`${API_BASE}/GetCalculationResult`, req);
}

export async function getCalculations(): Promise<Calculation[]> {
	return await apiFetch(`${API_BASE}/GetCalculations`);
}

export async function apiFetch(url: string) {
	const resp = await fetch(url, { method: 'GET' });
	return processResp(resp, url);
}

export async function apiPost(url: string, req: any) {
	const response = await fetch(url, {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(req),
	});

	return processResp(response, url);
}

function processResp(response: Response, url: string) {
	if (!response.ok) {
		throw new Error(`Failed to fetch ${url}`);
	}

	return response.json();
}
