import { useEffect, useState } from "react";
import {
  addTodo,
  deleteTodo,
  getTodos,
  updateTodo,
} from "./services/todoServices";

interface Todo {
  id: number;
  title: string;
  completed?: boolean;
}

export default function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newTitle, setNewTitle] = useState("");
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingTitle, setEditingTitle] = useState("");

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

  const startEditing = (id: number, title: string) => {
    setEditingId(id);
    setEditingTitle(title);
  };

  const handleUpdate = async (id: number) => {
    if (!editingTitle.trim()) return;

    try {
      const res = await updateTodo(id, { title: editingTitle });
      setTodos(todos.map((t) => (t.id === id ? res.data : t)));
      setEditingId(null);
      setEditingTitle("");
    } catch (err) {
      console.error("Error updating todos.", err);
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
            {editingId === todo.id ? (
              <>
                <input
                  type="text"
                  value={editingTitle}
                  onChange={(e) => setEditingTitle(e.target.value)}
                />
                <button onClick={() => handleUpdate(todo.id)}>Save</button>
                <button onClick={() => setEditingId(null)}>Cancel</button>
              </>
            ) : (
              <>
                {todo.title}{" "}
                <button onClick={() => startEditing(todo.id, todo.title)}>Edit</button>
                <button onClick={() => handleDelete(todo.id)}>Delete</button>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
