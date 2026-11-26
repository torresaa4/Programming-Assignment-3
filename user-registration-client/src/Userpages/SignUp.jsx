import { useEffect, useRef, useState } from 'react'
import $ from 'jquery'
import 'jquery-validation'

function SignUp({ setPage }) {
    // store info from user input
    const [name, setName] = useState('')
    const [email, setEmail] = useState('')
    const [phoneNumber, setPhoneNumber] = useState('')
    const [address, setAddress] = useState('')
    const [password, setPassword] = useState('')
    const [message, setMessage] = useState('')
    const [isSubmitting, setIsSubmitting] = useState(false)
    const submitRef = useRef(null)

    submitRef.current = async () => {
        setIsSubmitting(true)
        setMessage('')

        try {
            const response = await fetch('http://localhost:5297/api/auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name,
                    email,
                    phoneNumber,
                    address,
                    password
                })
            })

            const result = await response.text()

            if (!response.ok) {
                setMessage(result || 'Registration failed')
                return
            }

            setMessage(result || 'User registered')
        } catch (error) {
            setMessage('Unable to connect to the registration server')
        } finally {
            setIsSubmitting(false)
        }
    }

    useEffect(() => {
        const $form = $('#signUpForm')

        $form.validate({
            errorClass: 'validation-error',
            errorElement: 'span',
            rules: {
                name: {
                    required: true,
                    minlength: 2
                },
                email: {
                    required: true,
                    email: true
                },
                phoneNumber: {
                    required: true,
                    digits: true,
                    minlength: 7,
                    maxlength: 15
                },
                address: {
                    required: true,
                    minlength: 5
                },
                password: {
                    required: true,
                    minlength: 6,
                    maxlength: 15
                }
            },
            messages: {
                name: {
                    required: 'Please enter your name',
                    minlength: 'Name must be at least 2 characters'
                },
                email: {
                    required: 'Please enter your email',
                    email: 'Please enter a valid email address'
                },
                phoneNumber: {
                    required: 'Please enter your phone number',
                    digits: 'Phone number can only contain digits',
                    minlength: 'Phone number must contain at least 7 digits',
                    maxlength: 'Phone number cannot exceed 15 digits'
                },
                address: {
                    required: 'Please enter your address',
                    minlength: 'Address must be at least 5 characters'
                },
                password: {
                    required: 'Please enter a password',
                    minlength: 'Password must be at least 6 characters',
                    maxlength: 'Password cannot exceed 15 characters'
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
            <h1>Sign Up</h1>

            <form id="signUpForm">
                <label>Name:</label>
                <input
                    type="text"
                    name="name"
                    value={name}
                    onChange={(event) => setName(event.target.value)}
                    required
                />
                <br></br>
                <br></br>

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

                <label>Phone Number:</label>
                <input
                    type="tel"
                    name="phoneNumber"
                    value={phoneNumber}
                    onChange={(event) => setPhoneNumber(event.target.value)}
                    required
                />
                <br></br>
                <br></br>

                <label>Address:</label>
                <input
                    type="text"
                    name="address"
                    value={address}
                    onChange={(event) => setAddress(event.target.value)}
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
                    minLength={6}
                    maxLength={15}
                    required
                />
                <br></br>
                <br></br>

                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? 'Signing Up...' : 'Sign Up'}
                </button>
            </form>

            {message && <p>{message}</p>}

            <button type="button" onClick={() => setPage('home')}>
                Home page
            </button>
        </>
    )
}

export default SignUp