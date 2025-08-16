import axios from "axios";

const API_URL = "https://localhost:7126/api/ToDo";


export const getTodos = () => 
    axios.get(API_URL);

export const addTodo = (todo: {title: string, isCompleted: boolean}) => 
    axios.post(API_URL, todo);

export const deleteTodo = (id: number) => 
    axios.delete(`${API_URL}/${id}`);

export const updateTodo = (id: number, data: object) => 
    axios.put(`${API_URL}/${id}`, data);