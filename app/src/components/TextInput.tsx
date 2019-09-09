import styled from 'styled-components'
import { flexbox, space, FlexboxProps, SpaceProps } from 'styled-system'

const TextInput = styled.input<FlexboxProps & SpaceProps>`
  ${flexbox}
  ${space}

  padding: 5px;

  border: 1px solid ${props => props.theme.colors.placeholder};
  outline: none;
  border-radius: 3px;
`

export default TextInput
