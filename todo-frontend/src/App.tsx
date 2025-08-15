import { useEffect, useState } from "react";
import "./App.css";
import { addTodo, deleteTodo, getTodos } from "./services/todoServices";

interface Todo {
  id: number;
  title: string;
  completed?: boolean;
}

export default function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newTitle, setNewTitle] = useState("");

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      const res = await getTodos();
      setTodos(res.data);
    } catch (err) {
      console.error("Error fetching todos:", err);
    }
  };

  const handleAdd = async () => {
    if (!newTitle.trim()) return;

    try {
      const res = await addTodo({ title: newTitle });
      setTodos([...todos, res.data]);
      setNewTitle("");
    } catch (err) {
      console.error("Error adding Todos:", err);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteTodo(id);
      setTodos(todos.filter((t) => t.id !== id));
    } catch (err) {
      console.error("Error deleting todo:", err);
    }
  };

  return (
    <div style={{ padding: "20px", fontFamily: "Arial" }}>
      <h1>My To-Do list</h1>

      {/* Add todo */}

      <div>
        <input
          type="text"
          placeholder="Enter new todo"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
        />
        <button onClick={handleAdd}>Add</button>
      </div>

      {/* List todos */}
      
        <ul>
          {todos.map((todo) => (
            <li key={todo.id}>
              {todo.title}{" "}
              <button onClick={()=> handleDelete(todo.id)}>Delete</button>
            </li>
          ))}
        </ul>
    </div>
  );
}
