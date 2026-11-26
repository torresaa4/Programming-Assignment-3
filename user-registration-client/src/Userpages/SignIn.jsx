import { useEffect, useRef, useState } from 'react'
import $ from 'jquery'
import 'jquery-validation'

function SignIn({ setPage }) {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [message, setMessage] = useState('')
    const [isSubmitting, setIsSubmitting] = useState(false)
    const submitRef = useRef(null)

    submitRef.current = async () => {
        setIsSubmitting(true)
        setMessage('')

        try {
            const response = await fetch('http://localhost:5297/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email, password })
            })

            const result = await response.text()

            if (!response.ok) {
                setMessage(result || 'Sign in failed')
                return
            }

            setMessage(result || 'Login Successful')
        } catch (error) {
            setMessage('Unable to connect to the sign-in server')
        } finally {
            setIsSubmitting(false)
        }
    }

    useEffect(() => {
        const $form = $('#signInForm')

        $form.validate({
            errorClass: 'validation-error',
            errorElement: 'span',
            rules: {
                email: {
                    required: true,
                    email: true
                },
                password: {
                    required: true
                }
            },
            messages: {
                email: {
                    required: 'Please enter your email',
                    email: 'Please enter a valid email address'
                },
                password: {
                    required: 'Please enter your password'
                }
            },
            submitHandler: () => {
                void submitRef.current()
            }
        })

        return () => {
            $form.validate('destroy')
        }
    }, [])

    return (
        <>
            <h1>Sign In</h1>

            <form id="signInForm">
                <label>Email:</label>
                <input
                    type="email"
                    name="email"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    required
                />
                <br></br>
                <br></br>

                <label>Password:</label>
                <input
                    type="password"
                    name="password"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    required
                />
                <br></br>

                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? 'Signing In...' : 'Sign In'}
                </button>
            </form>

            {message && <p>{message}</p>}

            <button type="button" onClick={() => setPage('home')}>
                Home page
            </button>
        </>
    )
}

export default SignIn