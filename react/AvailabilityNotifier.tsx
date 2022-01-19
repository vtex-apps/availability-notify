import React, { useState } from 'react'
import type { FormEvent, ChangeEvent } from 'react'
import { useIntl } from 'react-intl'
import { useMutation } from 'react-apollo'
import { Button, Input } from 'vtex.styleguide'
import { useProduct } from 'vtex.product-context'
// import type { Seller } from 'vtex.product-context'
import { useRuntime } from 'vtex.render-runtime'

import ADD_TO_AVAILABILITY_SUBSCRIBER_MUTATION from './graphql/addToAvailabilityNotifierMutation.gql'
import styles from './AvailabilityNotifier.css'
import { getDefaultSeller } from './utils/sellers'

interface MutationVariables {
  skuId: string
  name: string
  email: string
  locale: string
  sellerObj: SellerObj
}

interface Props {
  /* Product's availability */
  available?: boolean
  /* SKU id to notify to */
  skuId?: string
}

interface SellerObj {
  sellerId: string | undefined
  sellerName: string | undefined
  addToCartLink: string | undefined
  sellerDefault: boolean | undefined
}

const isAvailable = (commertialOffer?: any['commertialOffer']) => {
  return (
    commertialOffer &&
    (Number.isNaN(+commertialOffer.AvailableQuantity) ||
      commertialOffer.AvailableQuantity > 0)
  )
}

/**
 * Availability Subscriber Component.
 * A form where users can sign up to be alerted
 * when a product becomes available again
 */
function AvailabilityNotifier(props: Props) {
  const productContext = useProduct()
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [emailError, setEmailError] = useState(false)
  const [didBlurEmail, setDidBlurEmail] = useState(false)

  const [signUp, { loading, error, data }] = useMutation<
    unknown,
    MutationVariables
  >(ADD_TO_AVAILABILITY_SUBSCRIBER_MUTATION)

  const intl = useIntl()

  const seller = getDefaultSeller(productContext.selectedItem?.sellers) as
    | any
    | null

  const available = props.available ?? isAvailable(seller?.commertialOffer)
  const skuId = props.skuId ?? productContext?.selectedItem?.itemId
  const { locale } = useRuntime().culture
  // console.log('Seller =>', seller)
  // const sellerObj = seller as SellerObj
  const sellerObj = {
    sellerName: seller?.sellerName,
    sellerId: seller?.sellerId,
    addToCartLink: seller?.addToCartLink,
    sellerDefault: seller?.sellerDefault,
  }

  // Render component only if the product is out of stock
  if (available || !skuId) {
    return null
  }

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    const variables: MutationVariables = {
      skuId,
      name,
      email,
      locale,
      sellerObj: {
        sellerId: sellerObj.sellerId,
        sellerName: sellerObj.sellerName,
        addToCartLink: sellerObj.addToCartLink,
        sellerDefault: sellerObj.sellerDefault,
      },
    }

    // console.log(variables)
    const signUpMutationResult = await signUp({
      variables,
    })

    if (!signUpMutationResult.errors) {
      setName('')
      setEmail('')
    }

    const event = new CustomEvent('message:success', {
      detail: {
        success: true,
        message: intl.formatMessage({
          id: 'store/availability-notify.added-message',
        }),
      },
    })

    document.dispatchEvent(event)
  }

  const validateEmail = (newEmail: string) => {
    const emailRegex =
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

    setEmailError(!emailRegex.test(newEmail.toLowerCase()))
  }

  const handleEmailChange = (e: ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value)
    validateEmail(e.target.value)
  }

  const handleNameChange = (e: ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value)
  }

  let emailErrorMessage = ''

  if (didBlurEmail && emailError) {
    emailErrorMessage = intl.formatMessage({
      id: 'store/availability-notify.invalid-email',
    })
  }

  const isFormDisabled = name === '' || email === '' || emailError || loading

  return (
    <div className={styles.notiferContainer}>
      <div className={`${styles.title} t-body mb3`}>
        {intl.formatMessage({ id: 'store/availability-notify.title' })}
      </div>
      <div className={`${styles.notifyLabel} t-small fw3`}>
        {intl.formatMessage({
          id: 'store/availability-notify.notify-label',
        })}
      </div>
      <form className={`${styles.form} mb4`} onSubmit={e => handleSubmit(e)}>
        <div className={`${styles.content} flex-ns justify-between mt4 mw6`}>
          <div className={`${styles.input} ${styles.inputName} w-100 mr5 mb4`}>
            <Input
              name="name"
              type="text"
              placeholder={intl.formatMessage({
                id: 'store/availability-notify.name-placeholder',
              })}
              value={name}
              onChange={handleNameChange}
            />
          </div>
          <div className={`${styles.input} ${styles.inputEmail} w-100 mr5 mb4`}>
            <Input
              name="email"
              type="text"
              placeholder={intl.formatMessage({
                id: 'store/availability-notify.email-placeholder',
              })}
              value={email}
              onChange={handleEmailChange}
              onBlur={() => setDidBlurEmail(true)}
              error={didBlurEmail && emailError}
              errorMessage={emailErrorMessage}
            />
          </div>
          <div className={`${styles.submit} flex items-center mb4`}>
            <Button
              type="submit"
              variation="primary"
              size="small"
              disabled={isFormDisabled}
              isLoading={loading}
            >
              {intl.formatMessage({
                id: 'store/availability-notify.send-label',
              })}
            </Button>
          </div>
        </div>
        {!error && data && (
          <div className={`${styles.success} t-body c-success`}>
            {intl.formatMessage({
              id: 'store/availability-notify.added-message',
            })}
          </div>
        )}
        {error && (
          <div className={`${styles.error} c-danger`}>
            {intl.formatMessage({
              id: 'store/availability-notify.error-message',
            })}
          </div>
        )}
      </form>
    </div>
  )
}

export default AvailabilityNotifier
