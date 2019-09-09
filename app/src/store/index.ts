import { createStore, createTypedHooks } from 'easy-peasy'
import storeModel, { StoreModel } from './models'

const store = createStore(storeModel)

export default store

const typedHooks = createTypedHooks<StoreModel>()

// export the typed hooks
export const useStoreActions = typedHooks.useStoreActions
export const useStoreDispatch = typedHooks.useStoreDispatch
export const useStoreState = typedHooks.useStoreState
