import axios from 'axios';

const anonymousAxios = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL + 'api/'
});

export default anonymousAxios;