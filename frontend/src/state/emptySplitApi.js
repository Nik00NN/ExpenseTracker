import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

console.log(444, import.meta.env.VITE_API_URL);
const api = createApi({
	baseQuery: fetchBaseQuery({
		baseUrl: import.meta.env.VITE_API_URL,
	}),
	endpoints: () => ({}),
});

export default api;
